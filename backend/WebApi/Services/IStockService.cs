namespace WebApi.Services;

/// <summary>
/// Interfaz para gestionar operaciones de stock de productos.
/// Servicio interno sin endpoints públicos.
/// </summary>
public interface IStockService
{
    /// <summary>
    /// Aumenta el stock disponible de un producto (ingreso de mercadería).
    /// </summary>
    /// <param name="productoId">ID del producto</param>
    /// <param name="cantidad">Cantidad a ingresar</param>
    /// <param name="razon">Motivo del ingreso (compra, devolución, ajuste, etc.)</param>
    /// <param name="userId">ID del usuario que realiza el movimiento</param>
    /// <returns>True si se completó exitosamente</returns>
    Task<bool> IngresarProductoAsync(long productoId, long cantidad, string razon, long userId);

    /// <summary>
    /// Disminuye el stock disponible de un producto (egreso de mercadería).
    /// </summary>
    /// <param name="productoId">ID del producto</param>
    /// <param name="cantidad">Cantidad a egresar</param>
    /// <param name="razon">Motivo del egreso (venta, rotura, pérdida, etc.)</param>
    /// <param name="userId">ID del usuario que realiza el movimiento</param>
    /// <returns>True si se completó exitosamente, false si no hay suficiente stock</returns>
    Task<bool> EgresarProductoAsync(long productoId, long cantidad, string razon, long userId);

    /// <summary>
    /// Obtiene la información actual del stock de un producto.
    /// </summary>
    /// <param name="productoId">ID del producto</param>
    /// <returns>Stock del producto o null si no existe</returns>
    Task<Models.Stock?> ObtenerStockAsync(long productoId);

    /// <summary>
    /// Verifica si hay suficiente stock disponible para una cantidad.
    /// </summary>
    /// <param name="productoId">ID del producto</param>
    /// <param name="cantidad">Cantidad a verificar</param>
    /// <returns>True si hay suficiente stock</returns>
    Task<bool> HaySufficienteStockAsync(long productoId, long cantidad);

    /// <summary>
    /// Obtiene el historial de movimientos de un producto.
    /// </summary>
    /// <param name="productoId">ID del producto</param>
    /// <param name="limit">Cantidad máxima de registros a devolver</param>
    /// <returns>Lista de movimientos ordenados por fecha descendente</returns>
    Task<IEnumerable<Models.MovimientoStock>> ObtenerMovimientosAsync(long productoId, int limit = 50);
}
