using WebApi.Models;

namespace WebApi.Services;

/// <summary>
/// Interfaz para gestionar operaciones relacionadas con ventas.
/// </summary>
public interface IVentaService
{
    /// <summary>
    /// Crea una nueva venta con sus detalles y actualiza el stock automáticamente.
    /// </summary>
    /// <param name="venta">Datos de la venta a crear</param>
    /// <param name="detalles">Lista de detalles de la venta (productos y cantidades)</param>
    /// <param name="userId">ID del usuario que realiza la venta</param>
    /// <returns>ID de la venta creada, o -1 si falló</returns>
    Task<long> CrearVentaAsync(Venta venta, List<DetalleVenta> detalles, long userId);

    /// <summary>
    /// Obtiene una venta con todos sus detalles.
    /// </summary>
    /// <param name="ventaId">ID de la venta</param>
    /// <returns>Venta con detalles o null si no existe</returns>
    Task<Venta?> ObtenerVentaAsync(long ventaId);

    /// <summary>
    /// Lista todas las ventas de un usuario.
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <returns>Enumerable de ventas</returns>
    Task<IEnumerable<Venta>> ListarVentasUsuarioAsync(long userId);

    /// <summary>
    /// Anula una venta y revierte los movimientos de stock.
    /// </summary>
    /// <param name="ventaId">ID de la venta a anular</param>
    /// <param name="userId">ID del usuario que anula la venta</param>
    /// <returns>True si se anuló exitosamente</returns>
    Task<bool> AnularVentaAsync(long ventaId, long userId);
}
