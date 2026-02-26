using WebApi.Models;

namespace WebApi.Services;

/// <summary>
/// Interfaz para gestionar operaciones de pagos en ventas.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Registra un nuevo pago para una venta.
    /// </summary>
    /// <param name="pago">Datos del pago</param>
    /// <param name="userId">ID del usuario que registra el pago</param>
    /// <returns>ID del pago registrado, o -1 si falló</returns>
    Task<long> RegistrarPagoAsync(Pago pago, long userId);

    /// <summary>
    /// Obtiene un pago específico.
    /// </summary>
    /// <param name="pagoId">ID del pago</param>
    /// <returns>Pago o null si no existe</returns>
    Task<Pago?> ObtenerPagoAsync(long pagoId);

    /// <summary>
    /// Lista todos los pagos de una venta.
    /// </summary>
    /// <param name="ventaId">ID de la venta</param>
    /// <returns>Enumerable de pagos</returns>
    Task<IEnumerable<Pago>> ObtenerPagosPorVentaAsync(long ventaId);

    /// <summary>
    /// Obtiene el estado de pago de una venta (monto pagado vs total).
    /// </summary>
    /// <param name="ventaId">ID de la venta</param>
    /// <returns>Tupla con (MontoPagado, Total, Saldo)</returns>
    Task<(decimal MontoPagado, decimal Total, decimal Saldo)> ObtenerEstadoPagoVentaAsync(long ventaId);

    /// <summary>
    /// Confirma un pago (marcar como confirmado).
    /// </summary>
    /// <param name="pagoId">ID del pago</param>
    /// <param name="userId">ID del usuario que confirma</param>
    /// <returns>True si se confirmó exitosamente</returns>
    Task<bool> ConfirmarPagoAsync(long pagoId, long userId);

    /// <summary>
    /// Anula un pago registrado.
    /// </summary>
    /// <param name="pagoId">ID del pago</param>
    /// <param name="userId">ID del usuario que anula</param>
    /// <returns>True si se anuló exitosamente</returns>
    Task<bool> AnularPagoAsync(long pagoId, long userId);

    /// <summary>
    /// Obtiene el resumen de pagos por método en un rango de fechas.
    /// </summary>
    /// <param name="desde">Fecha inicio</param>
    /// <param name="hasta">Fecha fin</param>
    /// <returns>Resumen de pagos agrupados por método</returns>
    Task<IEnumerable<(string MetodoPago, decimal Total, int Cantidad)>> ObtenerResumenPagosAsync(DateTime desde, DateTime hasta);
}
