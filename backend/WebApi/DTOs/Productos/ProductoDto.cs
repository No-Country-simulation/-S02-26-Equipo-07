namespace WebApi.DTOs.Productos
{
    public class ProductoDto
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
        public string? CategoriaNombre { get; set; }
    }
}