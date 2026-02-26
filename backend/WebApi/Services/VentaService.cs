using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Services;

public class VentaService : IVentaService
{
    private readonly NC07WebAppContext _context;
    private readonly IStockService _stockService;
    private readonly ILogger<VentaService> _logger;

    public VentaService(NC07WebAppContext context, IStockService stockService, ILogger<VentaService> logger)
    {
        _context = context;
        _stockService = stockService;
        _logger = logger;
    }

    /// <summary>
    /// Crea una nueva venta con sus detalles y actualiza el stock automáticamente.
    /// </summary>
    public async Task<long> CrearVentaAsync(Venta venta, List<DetalleVenta> detalles, long userId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Validar que hay detalles
            if (detalles == null || detalles.Count == 0)
            {
                _logger.LogWarning("Intento de crear venta sin detalles");
                return -1;
            }

            // Validar stock disponible para todos los productos antes de hacer cambios
            foreach (var detalle in detalles)
            {
                var hayStock = await _stockService.HaySufficienteStockAsync(
                    detalle.ProductoId, 
                    detalle.Cantidad);

                if (!hayStock)
                {
                    _logger.LogWarning($"Stock insuficiente para producto {detalle.ProductoId}");
                    return -1;
                }
            }

            // Asignar datos de la venta
            venta.UserId = userId;
            venta.FechaVenta = DateTime.UtcNow;
            venta.FechaCreacion = DateTime.UtcNow;
            venta.Estado = "COMPLETADA"; // Por defecto

            // Agregar venta
            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            // Procesar detalles y descontar stock
            foreach (var detalle in detalles)
            {
                detalle.VentaId = venta.Id;
                _context.DetallesVenta.Add(detalle);

                // Descontar del stock
                var egresoExitoso = await _stockService.EgresarProductoAsync(
                    detalle.ProductoId,
                    detalle.Cantidad,
                    $"VENTA #{venta.Id}",
                    userId);

                if (!egresoExitoso)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError($"Error al descontar stock del producto {detalle.ProductoId}");
                    return -1;
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation($"Venta #{venta.Id} creada exitosamente con {detalles.Count} detalles");
            return venta.Id;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError($"Error al crear venta: {ex.Message}");
            return -1;
        }
    }

    /// <summary>
    /// Obtiene una venta con todos sus detalles.
    /// </summary>
    public async Task<Venta?> ObtenerVentaAsync(long ventaId)
    {
        return await _context.Ventas
            .Include(v => v.User)
            .Include(v => v.DetallesVenta)
                .ThenInclude(dv => dv.Producto)
            .Include(v => v.Pagos)
            .FirstOrDefaultAsync(v => v.Id == ventaId);
    }

    /// <summary>
    /// Lista todas las ventas de un usuario.
    /// </summary>
    public async Task<IEnumerable<Venta>> ListarVentasUsuarioAsync(long userId)
    {
        return await _context.Ventas
            .Where(v => v.UserId == userId)
            .Include(v => v.DetallesVenta)
                .ThenInclude(dv => dv.Producto)
            .OrderByDescending(v => v.FechaVenta)
            .ToListAsync();
    }

    /// <summary>
    /// Anula una venta y revierte los movimientos de stock.
    /// </summary>
    public async Task<bool> AnularVentaAsync(long ventaId, long userId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var venta = await _context.Ventas
                .Include(v => v.DetallesVenta)
                .FirstOrDefaultAsync(v => v.Id == ventaId);

            if (venta == null)
            {
                _logger.LogWarning($"Venta {ventaId} no encontrada");
                return false;
            }

            // Revertir movimientos de stock
            foreach (var detalle in venta.DetallesVenta)
            {
                var ingresoExitoso = await _stockService.IngresarProductoAsync(
                    detalle.ProductoId,
                    detalle.Cantidad,
                    $"ANULACIÓN VENTA #{ventaId}",
                    userId);

                if (!ingresoExitoso)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError($"Error al reverting stock del producto {detalle.ProductoId}");
                    return false;
                }
            }

            // Marcar venta como anulada
            venta.Estado = "ANULADA";
            venta.FechaActualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation($"Venta #{ventaId} anulada exitosamente");
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError($"Error al anular venta: {ex.Message}");
            return false;
        }
    }
}
