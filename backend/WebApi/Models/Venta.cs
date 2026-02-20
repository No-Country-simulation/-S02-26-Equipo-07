using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Venta
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

    public virtual User User { get; set; } = null!;

    public virtual ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
