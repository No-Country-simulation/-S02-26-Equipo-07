using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Caballo
{
    public long Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Raza { get; set; } = null!;

    public decimal AlturaCm { get; set; }

    public string TipoTorso { get; set; } = null!;

    public decimal Anchura { get; set; }

    public short Edad { get; set; }

    public string Musculatura { get; set; } = null!;

    public long UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
