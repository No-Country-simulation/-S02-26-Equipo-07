namespace WebApi.DTOs.Ventas;

/// <summary>
/// DTO para crear una venta con sus detalles.
/// </summary>
public class CreateVentaDto
{
    public DateTime FechaVenta { get; set; }
    
    public decimal SubTotal { get; set; }
    
    public decimal Descuento { get; set; }
    
    public decimal IVA { get; set; }
    
    public decimal Total { get; set; }
    
    public string? Observaciones { get; set; }
    
    /// <summary>
    /// Lista de detalles de la venta (productos y cantidades).
    /// </summary>
    public List<CreateDetalleVentaDto> Detalles { get; set; } = new();
}

/// <summary>
/// DTO para crear un detalle de venta.
/// </summary>
public class CreateDetalleVentaDto
{
    public long ProductoId { get; set; }
    
    public long Cantidad { get; set; }
    
    public decimal PrecioUnitario { get; set; }
    
    public decimal Descuento { get; set; }
    
    public decimal Subtotal { get; set; }
}

/// <summary>
/// DTO para obtener información de una venta.
/// </summary>
public class GetVentaDto
{
    public long Id { get; set; }
    
    public long UserId { get; set; }
    
    public DateTime FechaVenta { get; set; }
    
    public decimal SubTotal { get; set; }
    
    public decimal Descuento { get; set; }
    
    public decimal IVA { get; set; }
    
    public decimal Total { get; set; }
    
    public string Estado { get; set; } = null!;
    
    public string? Observaciones { get; set; }
    
    public DateTime FechaCreacion { get; set; }
    
    public DateTime? FechaActualizacion { get; set; }
    
    public List<GetDetalleVentaDto> Detalles { get; set; } = new();
}

/// <summary>
/// DTO para obtener información de un detalle de venta.
/// </summary>
public class GetDetalleVentaDto
{
    public long Id { get; set; }
    
    public long ProductoId { get; set; }
    
    public string? NombreProducto { get; set; }
    
    public long Cantidad { get; set; }
    
    public decimal PrecioUnitario { get; set; }
    
    public decimal Descuento { get; set; }
    
    public decimal Subtotal { get; set; }
}
