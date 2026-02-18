using WebApi.DTOs.Categorias;
using WebApi.Models;

namespace WebApi.Services
{
    public interface ICategoriaService
    {
        Task<IEnumerable<GetCategoriaDto>> GetAllAsync();
        Task<GetCategoriaDto?> GetByIdAsync(long id);
        Task<IEnumerable<GetCategoriaDto>> GetAllChildrenById(long id);
        Task<GetCategoriaDto> CreateAsync(CreateCategoriaDto createCategoriaDto);
        Task<GetCategoriaDto?> UpdateAsync(long id, UpdateCategoriaDto updateCategoriaDto);
        Task<bool> DeleteAsync(long id);

    }
}
