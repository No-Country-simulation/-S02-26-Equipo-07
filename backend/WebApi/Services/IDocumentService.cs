using WebApi.Models;

namespace WebApi.Services;

/// <summary>
/// Interfaz para gestionar documentos comerciales (Facturas, Notas de Crédito/Débito).
/// </summary>
public interface IDocumentService
{
    // FACTURAS
    
    /// <summary>
    /// Emite una factura para una venta.
    /// </summary>
    /// <param name="factura">Datos de la factura</param>
    /// <param name="userId">ID del usuario que emite</param>
    /// <returns>ID de la factura emitida, o -1 si falló</returns>
    Task<long> EmitirFacturaAsync(Factura factura, long userId);

    /// <summary>
    /// Obtiene una factura con sus detalles.
    /// </summary>
    /// <param name="facturaId">ID de la factura</param>
    /// <returns>Factura o null</returns>
    Task<Factura?> ObtenerFacturaAsync(long facturaId);

    /// <summary>
    /// Obtiene una factura por su número.
    /// </summary>
    /// <param name="numeroFactura">Número de factura</param>
    /// <returns>Factura o null</returns>
    Task<Factura?> ObtenerFacturaPorNumeroAsync(string numeroFactura);

    /// <summary>
    /// Lista todas las facturas de una venta.
    /// </summary>
    /// <param name="ventaId">ID de la venta</param>
    /// <returns>Enumerable de facturas</returns>
    Task<IEnumerable<Factura>> ObtenerFacturasPorVentaAsync(long ventaId);

    /// <summary>
    /// Anula una factura (marca como anulada).
    /// </summary>
    /// <param name="facturaId">ID de la factura</param>
    /// <param name="userId">ID del usuario</param>
    /// <returns>True si se anuló exitosamente</returns>
    Task<bool> AnularFacturaAsync(long facturaId, long userId);

    // NOTAS DE CRÉDITO

    /// <summary>
    /// Emite una nota de crédito (devolución/descuento).
    /// </summary>
    /// <param name="notaCredito">Datos de la nota de crédito</param>
    /// <param name="userId">ID del usuario que emite</param>
    /// <returns>ID de la nota emitida, o -1 si falló</returns>
    Task<long> EmitirNotaCreditoAsync(NotaCredito notaCredito, long userId);

    /// <summary>
    /// Obtiene una nota de crédito.
    /// </summary>
    /// <param name="notaCreditoId">ID de la nota</param>
    /// <returns>Nota de crédito o null</returns>
    Task<NotaCredito?> ObtenerNotaCreditoAsync(long notaCreditoId);

    /// <summary>
    /// Lista todas las notas de crédito de una factura.
    /// </summary>
    /// <param name="facturaId">ID de la factura</param>
    /// <returns>Enumerable de notas</returns>
    Task<IEnumerable<NotaCredito>> ObtenerNotasCreditoPorFacturaAsync(long facturaId);

    /// <summary>
    /// Anula una nota de crédito.
    /// </summary>
    /// <param name="notaCreditoId">ID de la nota</param>
    /// <param name="userId">ID del usuario</param>
    /// <returns>True si se anuló exitosamente</returns>
    Task<bool> AnularNotaCreditoAsync(long notaCreditoId, long userId);

    // NOTAS DE DÉBITO

    /// <summary>
    /// Emite una nota de débito (recargo).
    /// </summary>
    /// <param name="notaDebito">Datos de la nota de débito</param>
    /// <param name="userId">ID del usuario que emite</param>
    /// <returns>ID de la nota emitida, o -1 si falló</returns>
    Task<long> EmitirNotaDebitoAsync(NotaDebito notaDebito, long userId);

    /// <summary>
    /// Obtiene una nota de débito.
    /// </summary>
    /// <param name="notaDebitoId">ID de la nota</param>
    /// <returns>Nota de débito o null</returns>
    Task<NotaDebito?> ObtenerNotaDebitoAsync(long notaDebitoId);

    /// <summary>
    /// Lista todas las notas de débito de una factura.
    /// </summary>
    /// <param name="facturaId">ID de la factura</param>
    /// <returns>Enumerable de notas</returns>
    Task<IEnumerable<NotaDebito>> ObtenerNotasDebitoPorFacturaAsync(long facturaId);

    /// <summary>
    /// Anula una nota de débito.
    /// </summary>
    /// <param name="notaDebitoId">ID de la nota</param>
    /// <param name="userId">ID del usuario</param>
    /// <returns>True si se anuló exitosamente</returns>
    Task<bool> AnularNotaDebitoAsync(long notaDebitoId, long userId);

    // UTILIDADES

    /// <summary>
    /// Genera el próximo número de documento (factura, nota, etc.).
    /// </summary>
    /// <param name="tipoDocumento">Tipo: FACTURA, NC, ND</param>
    /// <returns>Número secuencial</returns>
    Task<string> GenerarNumeroDocumentoAsync(string tipoDocumento);

    /// <summary>
    /// Obtiene resumen de documentos en un rango de fechas.
    /// </summary>
    /// <param name="desde">Fecha inicio</param>
    /// <param name="hasta">Fecha fin</param>
    /// <returns>Tupla con conteos y totales</returns>
    Task<(int TotalFacturas, decimal MontoFacturas, int TotalNotas, decimal MontoNotas)> ObtenerResumenDocumentosAsync(DateTime desde, DateTime hasta);
}
