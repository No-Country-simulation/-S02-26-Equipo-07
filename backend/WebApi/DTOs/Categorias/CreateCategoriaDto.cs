namespace WebApi.DTOs.Categorias
{
    public class CreateCategoriaDto
    {
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public long? CategoriaPadre { get; set; }
    }
}