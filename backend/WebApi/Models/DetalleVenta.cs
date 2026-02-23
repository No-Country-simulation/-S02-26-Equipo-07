using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class DetalleVenta
{
    public long Id { get; set; }

    public long VentaId { get; set; }

    public long ProductoId { get; set; }

    public long Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Descuento { get; set; }

    public decimal Subtotal { get; set; }

    public virtual Venta Venta { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;
}
