using WebApi.DTOs.Categorias;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<GetCategoriaDto> CreateAsync(CreateCategoriaDto createCategoriaDto)
        {
            var entity = new Categorium
            {
                Nombre = createCategoriaDto.Nombre,
                Descripcion = createCategoriaDto.Descripcion,
                CategoriaPadre = createCategoriaDto.CategoriaPadre
            };

            await _categoriaRepository.AddAsync(entity);

            return MapToDto(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _categoriaRepository.GetByIdAsync(id);
            if (entity == null) return false;

            await _categoriaRepository.DeleteAsync(entity);
            return true;
        }

        public async Task<IEnumerable<GetCategoriaDto>> GetAllAsync()
        {
            var items = await _categoriaRepository.GetAllAsync();
            return items.Select(MapToDto);
        }

        public async Task<IEnumerable<GetCategoriaDto>> GetAllChildrenById(long id)
        {
            var children = await _categoriaRepository.FindAsync(c => c.CategoriaPadre == id);
            return children.Select(MapToDto);
        }

        public async Task<GetCategoriaDto?> GetByIdAsync(long id)
        {
            var item = await _categoriaRepository.GetByIdAsync(id);
            return item == null ? null : MapToDto(item);
        }

        public async Task<GetCategoriaDto?> UpdateAsync(long id, UpdateCategoriaDto updateCategoriaDto)
        {
            var entity = await _categoriaRepository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.Nombre = updateCategoriaDto.Nombre;
            entity.Descripcion = updateCategoriaDto.Descripcion;
            entity.CategoriaPadre = updateCategoriaDto.CategoriaPadre;

            await _categoriaRepository.UpdateAsync(entity);

            return MapToDto(entity);
        }

        private static GetCategoriaDto MapToDto(Categorium c)
        {
            return new GetCategoriaDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                CategoriaPadre = c.CategoriaPadre
            };
        }
    }
}
