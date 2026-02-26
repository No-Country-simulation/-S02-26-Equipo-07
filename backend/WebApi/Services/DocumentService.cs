using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Services;

public class DocumentService : IDocumentService
{
    private readonly NC07WebAppContext _context;
    private readonly ILogger<DocumentService> _logger;

    public DocumentService(NC07WebAppContext context, ILogger<DocumentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // ============ FACTURAS ============

    /// <summary>
    /// Emite una factura para una venta.
    /// </summary>
    public async Task<long> EmitirFacturaAsync(Factura factura, long userId)
    {
        try
        {
            if (factura.VentaId <= 0)
            {
                _logger.LogWarning("Intento de emitir factura sin venta asociada");
                return -1;
            }

            // Verificar que la venta existe
            var venta = await _context.Ventas.FirstOrDefaultAsync(v => v.Id == factura.VentaId);
            if (venta == null)
            {
                _logger.LogWarning($"Venta {factura.VentaId} no encontrada");
                return -1;
            }

            // Verificar que no existe factura previa para esta venta
            var facturaExistente = await _context.Facturas
                .FirstOrDefaultAsync(f => f.VentaId == factura.VentaId && f.Estado != "ANULADA");

            if (facturaExistente != null)
            {
                _logger.LogWarning($"Ya existe factura {facturaExistente.NumeroFactura} para la venta {factura.VentaId}");
                return -1;
            }

            factura.FechaEmision = DateTime.UtcNow;
            factura.Estado = "VIGENTE";

            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Factura #{factura.NumeroFactura} emitida para venta #{factura.VentaId}");
            return factura.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al emitir factura: {ex.Message}");
            return -1;
        }
    }

    /// <summary>
    /// Obtiene una factura con sus detalles.
    /// </summary>
    public async Task<Factura?> ObtenerFacturaAsync(long facturaId)
    {
        return await _context.Facturas
            .FirstOrDefaultAsync(f => f.Id == facturaId);
    }

    /// <summary>
    /// Obtiene una factura por su número.
    /// </summary>
    public async Task<Factura?> ObtenerFacturaPorNumeroAsync(string numeroFactura)
    {
        return await _context.Facturas
            .FirstOrDefaultAsync(f => f.NumeroFactura == numeroFactura && f.Estado != "ANULADA");
    }

    /// <summary>
    /// Lista todas las facturas de una venta.
    /// </summary>
    public async Task<IEnumerable<Factura>> ObtenerFacturasPorVentaAsync(long ventaId)
    {
        return await _context.Facturas
            .Where(f => f.VentaId == ventaId)
            .OrderByDescending(f => f.FechaEmision)
            .ToListAsync();
    }

    /// <summary>
    /// Anula una factura (marca como anulada).
    /// </summary>
    public async Task<bool> AnularFacturaAsync(long facturaId, long userId)
    {
        try
        {
            var factura = await _context.Facturas.FirstOrDefaultAsync(f => f.Id == facturaId);
            if (factura == null)
            {
                _logger.LogWarning($"Factura {facturaId} no encontrada");
                return false;
            }

            if (factura.Estado == "ANULADA")
            {
                _logger.LogWarning($"Factura {facturaId} ya está anulada");
                return false;
            }

            factura.Estado = "ANULADA";
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Factura #{factura.NumeroFactura} anulada");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al anular factura: {ex.Message}");
            return false;
        }
    }

    // ============ NOTAS DE CRÉDITO ============

    /// <summary>
    /// Emite una nota de crédito (devolución/descuento).
    /// </summary>
    public async Task<long> EmitirNotaCreditoAsync(NotaCredito notaCredito, long userId)
    {
        try
        {
            if (notaCredito.FacturaId <= 0)
            {
                _logger.LogWarning("Intento de emitir nota de crédito sin factura asociada");
                return -1;
            }

            // Verificar que la factura existe
            var factura = await _context.Facturas.FirstOrDefaultAsync(f => f.Id == notaCredito.FacturaId);
            if (factura == null)
            {
                _logger.LogWarning($"Factura {notaCredito.FacturaId} no encontrada");
                return -1;
            }

            // Validar que el monto no exceda el de la factura
            if (notaCredito.Total > factura.Total)
            {
                _logger.LogWarning($"Monto de NC excede el total de factura. Factura: {factura.Total}, NC: {notaCredito.Total}");
                return -1;
            }

            notaCredito.FechaEmision = DateTime.UtcNow;
            notaCredito.FechaCreacion = DateTime.UtcNow;
            notaCredito.Estado = "VIGENTE";
            notaCredito.UserId = userId;

            _context.NotasCredito.Add(notaCredito);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Nota de Crédito #{notaCredito.NumeroNotaCredito} emitida para factura #{factura.NumeroFactura}");
            return notaCredito.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al emitir nota de crédito: {ex.Message}");
            return -1;
        }
    }

    /// <summary>
    /// Obtiene una nota de crédito.
    /// </summary>
    public async Task<NotaCredito?> ObtenerNotaCreditoAsync(long notaCreditoId)
    {
        return await _context.NotasCredito
            .Include(nc => nc.Factura)
            .Include(nc => nc.User)
            .FirstOrDefaultAsync(nc => nc.Id == notaCreditoId);
    }

    /// <summary>
    /// Lista todas las notas de crédito de una factura.
    /// </summary>
    public async Task<IEnumerable<NotaCredito>> ObtenerNotasCreditoPorFacturaAsync(long facturaId)
    {
        return await _context.NotasCredito
            .Where(nc => nc.FacturaId == facturaId && nc.Estado != "ANULADA")
            .OrderByDescending(nc => nc.FechaEmision)
            .ToListAsync();
    }

    /// <summary>
    /// Anula una nota de crédito.
    /// </summary>
    public async Task<bool> AnularNotaCreditoAsync(long notaCreditoId, long userId)
    {
        try
        {
            var notaCredito = await _context.NotasCredito.FirstOrDefaultAsync(nc => nc.Id == notaCreditoId);
            if (notaCredito == null)
            {
                _logger.LogWarning($"Nota de crédito {notaCreditoId} no encontrada");
                return false;
            }

            if (notaCredito.Estado == "ANULADA")
            {
                _logger.LogWarning($"Nota de crédito {notaCreditoId} ya está anulada");
                return false;
            }

            notaCredito.Estado = "ANULADA";
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Nota de Crédito #{notaCredito.NumeroNotaCredito} anulada");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al anular nota de crédito: {ex.Message}");
            return false;
        }
    }

    // ============ NOTAS DE DÉBITO ============

    /// <summary>
    /// Emite una nota de débito (recargo).
    /// </summary>
    public async Task<long> EmitirNotaDebitoAsync(NotaDebito notaDebito, long userId)
    {
        try
        {
            if (notaDebito.FacturaId <= 0)
            {
                _logger.LogWarning("Intento de emitir nota de débito sin factura asociada");
                return -1;
            }

            // Verificar que la factura existe
            var factura = await _context.Facturas.FirstOrDefaultAsync(f => f.Id == notaDebito.FacturaId);
            if (factura == null)
            {
                _logger.LogWarning($"Factura {notaDebito.FacturaId} no encontrada");
                return -1;
            }

            notaDebito.FechaEmision = DateTime.UtcNow;
            notaDebito.FechaCreacion = DateTime.UtcNow;
            notaDebito.Estado = "VIGENTE";
            notaDebito.UserId = userId;

            _context.NotasDebito.Add(notaDebito);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Nota de Débito #{notaDebito.NumeroNotaDebito} emitida para factura #{factura.NumeroFactura}");
            return notaDebito.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al emitir nota de débito: {ex.Message}");
            return -1;
        }
    }

    /// <summary>
    /// Obtiene una nota de débito.
    /// </summary>
    public async Task<NotaDebito?> ObtenerNotaDebitoAsync(long notaDebitoId)
    {
        return await _context.NotasDebito
            .Include(nd => nd.Factura)
            .Include(nd => nd.User)
            .FirstOrDefaultAsync(nd => nd.Id == notaDebitoId);
    }

    /// <summary>
    /// Lista todas las notas de débito de una factura.
    /// </summary>
    public async Task<IEnumerable<NotaDebito>> ObtenerNotasDebitoPorFacturaAsync(long facturaId)
    {
        return await _context.NotasDebito
            .Where(nd => nd.FacturaId == facturaId && nd.Estado != "ANULADA")
            .OrderByDescending(nd => nd.FechaEmision)
            .ToListAsync();
    }

    /// <summary>
    /// Anula una nota de débito.
    /// </summary>
    public async Task<bool> AnularNotaDebitoAsync(long notaDebitoId, long userId)
    {
        try
        {
            var notaDebito = await _context.NotasDebito.FirstOrDefaultAsync(nd => nd.Id == notaDebitoId);
            if (notaDebito == null)
            {
                _logger.LogWarning($"Nota de débito {notaDebitoId} no encontrada");
                return false;
            }

            if (notaDebito.Estado == "ANULADA")
            {
                _logger.LogWarning($"Nota de débito {notaDebitoId} ya está anulada");
                return false;
            }

            notaDebito.Estado = "ANULADA";
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Nota de Débito #{notaDebito.NumeroNotaDebito} anulada");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al anular nota de débito: {ex.Message}");
            return false;
        }
    }

    // ============ UTILIDADES ============

    /// <summary>
    /// Genera el próximo número de documento (factura, nota, etc.).
    /// </summary>
    public async Task<string> GenerarNumeroDocumentoAsync(string tipoDocumento)
    {
        try
        {
            string numero;

            switch (tipoDocumento.ToUpper())
            {
                case "FACTURA":
                    var maxFactura = await _context.Facturas
                        .Where(f => f.Estado != "ANULADA")
                        .MaxAsync(f => (long?)f.Id) ?? 0;
                    numero = $"FAC-{DateTime.UtcNow:yyyyMM}-{(maxFactura + 1):D6}";
                    break;

                case "NC":
                    var maxNC = await _context.NotasCredito
                        .Where(nc => nc.Estado != "ANULADA")
                        .MaxAsync(nc => (long?)nc.Id) ?? 0;
                    numero = $"NC-{DateTime.UtcNow:yyyyMM}-{(maxNC + 1):D6}";
                    break;

                case "ND":
                    var maxND = await _context.NotasDebito
                        .Where(nd => nd.Estado != "ANULADA")
                        .MaxAsync(nd => (long?)nd.Id) ?? 0;
                    numero = $"ND-{DateTime.UtcNow:yyyyMM}-{(maxND + 1):D6}";
                    break;

                default:
                    numero = $"DOC-{DateTime.UtcNow:yyyyMMddHHmmss}";
                    break;
            }

            return numero;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al generar número de documento: {ex.Message}");
            return string.Empty;
        }
    }

    /// <summary>
    /// Obtiene resumen de documentos en un rango de fechas.
    /// </summary>
    public async Task<(int TotalFacturas, decimal MontoFacturas, int TotalNotas, decimal MontoNotas)> ObtenerResumenDocumentosAsync(DateTime desde, DateTime hasta)
    {
        var facturas = await _context.Facturas
            .Where(f => f.FechaEmision >= desde && f.FechaEmision <= hasta && f.Estado != "ANULADA")
            .ToListAsync();

        var notas = await _context.NotasCredito
            .Where(nc => nc.FechaEmision >= desde && nc.FechaEmision <= hasta && nc.Estado != "ANULADA")
            .ToListAsync();

        var totalFacturas = facturas.Count;
        var montoFacturas = facturas.Sum(f => f.Total);
        var totalNotas = notas.Count;
        var montoNotas = notas.Sum(nc => nc.Total);

        return (totalFacturas, montoFacturas, totalNotas, montoNotas);
    }
}
