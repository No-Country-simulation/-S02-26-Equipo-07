using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Services;

public class PaymentService : IPaymentService
{
    private readonly NC07WebAppContext _context;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(NC07WebAppContext context, ILogger<PaymentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Registra un nuevo pago para una venta.
    /// </summary>
    public async Task<long> RegistrarPagoAsync(Pago pago, long userId)
    {
        try
        {
            if (pago.Monto <= 0)
            {
                _logger.LogWarning("Intento de registrar pago con monto inválido");
                return -1;
            }

            // Verificar que la venta existe
            var venta = await _context.Ventas.FirstOrDefaultAsync(v => v.Id == pago.VentaId);
            if (venta == null)
            {
                _logger.LogWarning($"Venta {pago.VentaId} no encontrada");
                return -1;
            }

            // Verificar que el monto no exceda el total de la venta
            var montoPagado = await _context.Pagos
                .Where(p => p.VentaId == pago.VentaId && p.Estado != "ANULADO")
                .SumAsync(p => p.Monto);

            if (montoPagado + pago.Monto > venta.Total)
            {
                _logger.LogWarning($"Monto de pago excede el total de la venta. Venta: {pago.VentaId}, Total: {venta.Total}, Ya pagado: {montoPagado}, Nuevo pago: {pago.Monto}");
                return -1;
            }

            pago.UserId = userId;
            pago.FechaPago = DateTime.UtcNow;
            pago.Estado = "REGISTRADO"; // Por defecto

            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();

            // Actualizar estado de la venta si está completamente pagada
            var totalPagado = montoPagado + pago.Monto;
            if (totalPagado >= venta.Total)
            {
                venta.Estado = "PAGADA";
                venta.FechaActualizacion = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Venta {pago.VentaId} marcada como PAGADA");
            }

            _logger.LogInformation($"Pago #{pago.Id} registrado para venta #{pago.VentaId}. Monto: {pago.Monto}");
            return pago.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al registrar pago: {ex.Message}");
            return -1;
        }
    }

    /// <summary>
    /// Obtiene un pago específico.
    /// </summary>
    public async Task<Pago?> ObtenerPagoAsync(long pagoId)
    {
        return await _context.Pagos
            .Include(p => p.Venta)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == pagoId);
    }

    /// <summary>
    /// Lista todos los pagos de una venta.
    /// </summary>
    public async Task<IEnumerable<Pago>> ObtenerPagosPorVentaAsync(long ventaId)
    {
        return await _context.Pagos
            .Where(p => p.VentaId == ventaId && p.Estado != "ANULADO")
            .Include(p => p.User)
            .OrderByDescending(p => p.FechaPago)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene el estado de pago de una venta (monto pagado vs total).
    /// </summary>
    public async Task<(decimal MontoPagado, decimal Total, decimal Saldo)> ObtenerEstadoPagoVentaAsync(long ventaId)
    {
        var venta = await _context.Ventas.FirstOrDefaultAsync(v => v.Id == ventaId);
        if (venta == null)
            return (0, 0, 0);

        var montoPagado = await _context.Pagos
            .Where(p => p.VentaId == ventaId && p.Estado != "ANULADO")
            .SumAsync(p => p.Monto);

        var saldo = venta.Total - montoPagado;

        return (montoPagado, venta.Total, saldo > 0 ? saldo : 0);
    }

    /// <summary>
    /// Confirma un pago (marcar como confirmado).
    /// </summary>
    public async Task<bool> ConfirmarPagoAsync(long pagoId, long userId)
    {
        try
        {
            var pago = await _context.Pagos.FirstOrDefaultAsync(p => p.Id == pagoId);
            if (pago == null)
            {
                _logger.LogWarning($"Pago {pagoId} no encontrado");
                return false;
            }

            if (pago.Estado == "CONFIRMADO")
            {
                _logger.LogWarning($"Pago {pagoId} ya está confirmado");
                return false;
            }

            pago.Estado = "CONFIRMADO";
            pago.FechaConfirmacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Pago #{pagoId} confirmado");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al confirmar pago: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Anula un pago registrado.
    /// </summary>
    public async Task<bool> AnularPagoAsync(long pagoId, long userId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var pago = await _context.Pagos.FirstOrDefaultAsync(p => p.Id == pagoId);
            if (pago == null)
            {
                _logger.LogWarning($"Pago {pagoId} no encontrado");
                return false;
            }

            if (pago.Estado == "ANULADO")
            {
                _logger.LogWarning($"Pago {pagoId} ya está anulado");
                return false;
            }

            pago.Estado = "ANULADO";

            var venta = await _context.Ventas.FirstOrDefaultAsync(v => v.Id == pago.VentaId);
            if (venta != null)
            {
                // Cambiar estado de venta si estaba pagada
                var montoPagadoRestante = await _context.Pagos
                    .Where(p => p.VentaId == pago.VentaId && p.Estado != "ANULADO" && p.Id != pagoId)
                    .SumAsync(p => p.Monto);

                if (montoPagadoRestante < venta.Total)
                {
                    venta.Estado = "COMPLETADA";
                    venta.FechaActualizacion = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            _logger.LogInformation($"Pago #{pagoId} anulado");
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError($"Error al anular pago: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Obtiene el resumen de pagos por método en un rango de fechas.
    /// </summary>
    public async Task<IEnumerable<(string MetodoPago, decimal Total, int Cantidad)>> ObtenerResumenPagosAsync(DateTime desde, DateTime hasta)
    {
        var resumen = await _context.Pagos
            .Where(p => p.FechaPago >= desde && p.FechaPago <= hasta && p.Estado != "ANULADO")
            .GroupBy(p => p.MetodoPago)
            .Select(g => new { 
                MetodoPago = g.Key, 
                Total = g.Sum(p => p.Monto),
                Cantidad = g.Count()
            })
            .OrderByDescending(x => x.Total)
            .ToListAsync();

        return resumen.Select(x => (x.MetodoPago, x.Total, x.Cantidad));
    }
}
