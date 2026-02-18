using WebApi.DTOs.Productos;

namespace WebApi.Services
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoDto>> GetAllAsync();
        Task<ProductoDto?> GetByIdAsync(long id);
        Task<IEnumerable<ProductoDto>> GetByCategoryAsync(long categoriaId);
        Task<ProductoDto> CreateAsync(CreateProductoDto dto);
        Task<ProductoDto?> UpdateAsync(long id, UpdateProductoDto dto);
        Task<bool> DeleteAsync(long id);
    }
}