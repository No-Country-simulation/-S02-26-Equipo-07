using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class NotaCredito
{
    public long Id { get; set; }

    public long FacturaId { get; set; }

    public string NumeroNotaCredito { get; set; } = null!;

    public string TipoNotaCredito { get; set; } = null!;

    public DateTime FechaEmision { get; set; }

    public string Razon { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string Estado { get; set; } = null!;

    public string? CAE { get; set; }

    public DateTime? FechaVencimientoCae { get; set; }

    public string CUITCliente { get; set; } = null!;

    public string NombreCliente { get; set; } = null!;

    public decimal Monto { get; set; }

    public decimal IVA { get; set; }

    public decimal PorcentajeIVA { get; set; }

    public string? IIBB { get; set; }

    public decimal Total { get; set; }

    public long? UserId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Factura Factura { get; set; } = null!;

    public virtual User? User { get; set; }
}
