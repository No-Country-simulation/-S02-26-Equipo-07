using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Producto
{
    public long Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Price { get; set; }

    public decimal? Descuento { get; set; }

    public string? Sku { get; set; }

    public string? Lote { get; set; }

    public decimal CostoUnitario { get; set; }

    public decimal? Iva { get; set; }

    public long? Categoria { get; set; }

    public virtual Categorium? CategoriaNavigation { get; set; }
}
