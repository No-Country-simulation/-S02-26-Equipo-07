using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Factura
{
    public long Id { get; set; }

    public long VentaId { get; set; }

    public string NumeroFactura { get; set; } = null!;

    public string TipoFactura { get; set; } = null!;

    public DateTime FechaEmision { get; set; }

    public DateTime? FechaVencimiento { get; set; }

    public string Estado { get; set; } = null!;

    public string CUIT { get; set; } = null!;

    public string CUITCliente { get; set; } = null!;

    public string TipoDocumentoCliente { get; set; } = null!;

    public string NumeroDocumentoCliente { get; set; } = null!;

    public string NombreCliente { get; set; } = null!;

    public string CondicionIva { get; set; } = null!;

    public string? CAE { get; set; }

    public DateTime? FechaVencimientoCae { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Descuento { get; set; }

    public decimal IVA { get; set; }

    public decimal PorcentajeIVA { get; set; }

    public string? IIBB { get; set; }

    public decimal Total { get; set; }

    public string? Observaciones { get; set; }

    public long? UserId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Venta Venta { get; set; } = null!;

    public virtual User? User { get; set; }
}
