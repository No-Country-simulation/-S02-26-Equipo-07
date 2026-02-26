namespace WebApi.DTOs.Pagos;

/// <summary>
/// DTO para registrar un pago.
/// </summary>
public class CreatePagoDto
{
    public long VentaId { get; set; }
    
    public decimal Monto { get; set; }
    
    public string MetodoPago { get; set; } = null!;
    
    public string? ReferenciaPago { get; set; }
    
    public string? Observaciones { get; set; }
}

/// <summary>
/// DTO para obtener información de un pago.
/// </summary>
public class GetPagoDto
{
    public long Id { get; set; }
    
    public long VentaId { get; set; }
    
    public decimal Monto { get; set; }
    
    public string MetodoPago { get; set; } = null!;
    
    public string? ReferenciaPago { get; set; }
    
    public string Estado { get; set; } = null!;
    
    public DateTime FechaPago { get; set; }
    
    public DateTime? FechaConfirmacion { get; set; }
    
    public string? Observaciones { get; set; }
}

/// <summary>
/// DTO para estado de pago de una venta.
/// </summary>
public class EstadoPagoDto
{
    public long VentaId { get; set; }
    
    public decimal MontoPagado { get; set; }
    
    public decimal Total { get; set; }
    
    public decimal Saldo { get; set; }
    
    public decimal Porcentaje => Total > 0 ? (MontoPagado / Total) * 100 : 0;
}

/// <summary>
/// DTO para resumen de pagos.
/// </summary>
public class ResumenPagosDto
{
    public string MetodoPago { get; set; } = null!;
    
    public decimal Total { get; set; }
    
    public int Cantidad { get; set; }
}
