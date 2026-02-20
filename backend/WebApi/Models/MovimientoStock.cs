using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class MovimientoStock
{
    public long Id { get; set; }

    public long StockId { get; set; }

    public string TipoMovimiento { get; set; } = null!;

    public long Cantidad { get; set; }

    public string? Razon { get; set; }

    public DateTime FechaMovimiento { get; set; }

    public long? UserId { get; set; }

    public virtual Stock Stock { get; set; } = null!;

    public virtual User? User { get; set; }
}
