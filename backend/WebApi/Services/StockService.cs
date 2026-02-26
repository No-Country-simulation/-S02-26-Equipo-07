using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Services;

public class StockService : IStockService
{
    private readonly NC07WebAppContext _context;
    private readonly ILogger<StockService> _logger;

    public StockService(NC07WebAppContext context, ILogger<StockService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Aumenta el stock disponible de un producto (ingreso de mercadería).
    /// </summary>
    public async Task<bool> IngresarProductoAsync(long productoId, long cantidad, string razon, long userId)
    {
        try
        {
            if (cantidad <= 0)
            {
                _logger.LogWarning($"Intento de ingreso con cantidad inválida: {cantidad}");
                return false;
            }

            var stock = await _context.Stocks
                .FirstOrDefaultAsync(s => s.ProductoId == productoId);

            if (stock == null)
            {
                _logger.LogWarning($"Stock no encontrado para producto: {productoId}");
                return false;
            }

            // Registrar movimiento
            var movimiento = new MovimientoStock
            {
                StockId = stock.Id,
                TipoMovimiento = "INGRESO",
                Cantidad = cantidad,
                Razon = razon,
                FechaMovimiento = DateTime.UtcNow,
                UserId = userId
            };

            stock.CantidadDisponible += cantidad;
            stock.UltimaActualizacion = DateTime.UtcNow;

            _context.MovimientosStock.Add(movimiento);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Ingreso registrado: Producto {productoId}, Cantidad {cantidad}, Razón: {razon}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al ingresar producto {productoId}: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Disminuye el stock disponible de un producto (egreso de mercadería).
    /// </summary>
    public async Task<bool> EgresarProductoAsync(long productoId, long cantidad, string razon, long userId)
    {
        try
        {
            if (cantidad <= 0)
            {
                _logger.LogWarning($"Intento de egreso con cantidad inválida: {cantidad}");
                return false;
            }

            var stock = await _context.Stocks
                .FirstOrDefaultAsync(s => s.ProductoId == productoId);

            if (stock == null)
            {
                _logger.LogWarning($"Stock no encontrado para producto: {productoId}");
                return false;
            }

            if (stock.CantidadDisponible < cantidad)
            {
                _logger.LogWarning($"Stock insuficiente. Producto: {productoId}, Disponible: {stock.CantidadDisponible}, Solicitado: {cantidad}");
                return false;
            }

            // Registrar movimiento
            var movimiento = new MovimientoStock
            {
                StockId = stock.Id,
                TipoMovimiento = "EGRESO",
                Cantidad = cantidad,
                Razon = razon,
                FechaMovimiento = DateTime.UtcNow,
                UserId = userId
            };

            stock.CantidadDisponible -= cantidad;
            stock.UltimaActualizacion = DateTime.UtcNow;

            _context.MovimientosStock.Add(movimiento);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Egreso registrado: Producto {productoId}, Cantidad {cantidad}, Razón: {razon}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al egresar producto {productoId}: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Obtiene la información actual del stock de un producto.
    /// </summary>
    public async Task<Stock?> ObtenerStockAsync(long productoId)
    {
        return await _context.Stocks
            .Include(s => s.Producto)
            .FirstOrDefaultAsync(s => s.ProductoId == productoId);
    }

    /// <summary>
    /// Verifica si hay suficiente stock disponible para una cantidad.
    /// </summary>
    public async Task<bool> HaySufficienteStockAsync(long productoId, long cantidad)
    {
        var stock = await _context.Stocks
            .FirstOrDefaultAsync(s => s.ProductoId == productoId);

        if (stock == null)
            return false;

        return stock.CantidadDisponible >= cantidad;
    }

    /// <summary>
    /// Obtiene el historial de movimientos de un producto.
    /// </summary>
    public async Task<IEnumerable<MovimientoStock>> ObtenerMovimientosAsync(long productoId, int limit = 50)
    {
        return await _context.MovimientosStock
            .Where(m => m.Stock.ProductoId == productoId)
            .OrderByDescending(m => m.FechaMovimiento)
            .Take(limit)
            .ToListAsync();
    }
}
