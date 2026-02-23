using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Pago
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

    public long? UserId { get; set; }

    public virtual Venta Venta { get; set; } = null!;

    public virtual User? User { get; set; }
}
