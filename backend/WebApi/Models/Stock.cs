using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Stock
{
    public long Id { get; set; }

    public long ProductoId { get; set; }

    public long CantidadDisponible { get; set; }

    public long CantidadReservada { get; set; }

    public long CantidadMinima { get; set; }

    public DateTime UltimaActualizacion { get; set; }

    public virtual Producto Producto { get; set; } = null!;

    public virtual ICollection<MovimientoStock> MovimientosStock { get; set; } = new List<MovimientoStock>();
}
