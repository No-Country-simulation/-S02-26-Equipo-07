using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Categorium
{
    public long Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public long? CategoriaPadre { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
