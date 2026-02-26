namespace WebApi.DTOs.Documentos;

/// <summary>
/// DTO para emitir una factura.
/// </summary>
public class CreateFacturaDto
{
    public long VentaId { get; set; }
    
    public string TipoFactura { get; set; } = null!; // A, B, C
    
    public string CUIT { get; set; } = null!;
    
    public string CUITCliente { get; set; } = null!;
    
    public string TipoDocumentoCliente { get; set; } = null!;
    
    public string NumeroDocumentoCliente { get; set; } = null!;
    
    public string NombreCliente { get; set; } = null!;
    
    public string CondicionIva { get; set; } = null!;
    
    public decimal Subtotal { get; set; }
    
    public decimal Descuento { get; set; }
    
    public decimal IVA { get; set; }
    
    public decimal PorcentajeIVA { get; set; }
    
    public string? IIBB { get; set; }
    
    public decimal Total { get; set; }
    
    public string? Observaciones { get; set; }
}

/// <summary>
/// DTO para obtener información de una factura.
/// </summary>
public class GetFacturaDto
{
    public long Id { get; set; }
    
    public long VentaId { get; set; }
    
    public string NumeroFactura { get; set; } = null!;
    
    public string TipoFactura { get; set; } = null!;
    
    public DateTime FechaEmision { get; set; }
    
    public DateTime? FechaVencimiento { get; set; }
    
    public string Estado { get; set; } = null!;
    
    public string NombreCliente { get; set; } = null!;
    
    public decimal Total { get; set; }
    
    public string? CAE { get; set; }
    
    public DateTime? FechaVencimientoCae { get; set; }
}

/// <summary>
/// DTO para emitir una nota de crédito.
/// </summary>
public class CreateNotaCreditoDto
{
    public long FacturaId { get; set; }
    
    public string TipoNotaCredito { get; set; } = null!;
    
    public string Razon { get; set; } = null!;
    
    public string? Descripcion { get; set; }
    
    public string CUITCliente { get; set; } = null!;
    
    public string NombreCliente { get; set; } = null!;
    
    public decimal Monto { get; set; }
    
    public decimal IVA { get; set; }
    
    public decimal PorcentajeIVA { get; set; }
    
    public string? IIBB { get; set; }
    
    public decimal Total { get; set; }
}

/// <summary>
/// DTO para obtener información de una nota de crédito.
/// </summary>
public class GetNotaCreditoDto
{
    public long Id { get; set; }
    
    public long FacturaId { get; set; }
    
    public string NumeroNotaCredito { get; set; } = null!;
    
    public DateTime FechaEmision { get; set; }
    
    public string Razon { get; set; } = null!;
    
    public string Estado { get; set; } = null!;
    
    public string NombreCliente { get; set; } = null!;
    
    public decimal Total { get; set; }
}

/// <summary>
/// DTO para emitir una nota de débito.
/// </summary>
public class CreateNotaDebitoDto
{
    public long FacturaId { get; set; }
    
    public string TipoNotaDebito { get; set; } = null!;
    
    public string Razon { get; set; } = null!;
    
    public string? Descripcion { get; set; }
    
    public string CUITCliente { get; set; } = null!;
    
    public string NombreCliente { get; set; } = null!;
    
    public decimal Monto { get; set; }
    
    public decimal IVA { get; set; }
    
    public decimal PorcentajeIVA { get; set; }
    
    public string? IIBB { get; set; }
    
    public decimal Total { get; set; }
}

/// <summary>
/// DTO para obtener información de una nota de débito.
/// </summary>
public class GetNotaDebitoDto
{
    public long Id { get; set; }
    
    public long FacturaId { get; set; }
    
    public string NumeroNotaDebito { get; set; } = null!;
    
    public DateTime FechaEmision { get; set; }
    
    public string Razon { get; set; } = null!;
    
    public string Estado { get; set; } = null!;
    
    public string NombreCliente { get; set; } = null!;
    
    public decimal Total { get; set; }
}

/// <summary>
/// DTO para resumen de documentos.
/// </summary>
public class ResumenDocumentosDto
{
    public int TotalFacturas { get; set; }
    
    public decimal MontoFacturas { get; set; }
    
    public int TotalNotas { get; set; }
    
    public decimal MontoNotas { get; set; }
}
