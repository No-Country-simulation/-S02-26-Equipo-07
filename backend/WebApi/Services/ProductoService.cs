using WebApi.DTOs.Productos;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;

        public ProductoService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public async Task<ProductoDto> CreateAsync(CreateProductoDto dto)
        {
            var entity = new Producto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Price = dto.Price,
                Descuento = dto.Descuento,
                Sku = dto.Sku,
                Lote = dto.Lote,
                CostoUnitario = dto.CostoUnitario,
                Iva = dto.Iva,
                Categoria = dto.Categoria
            };

            await _productoRepository.AddAsync(entity);
            return MapToDto(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _productoRepository.GetByIdAsync(id);
            if (entity == null) return false;
            await _productoRepository.DeleteAsync(entity);
            return true;
        }

        public async Task<IEnumerable<ProductoDto>> GetAllAsync()
        {
            var items = await _productoRepository.GetAllAsync();
            return items.Select(MapToDto);
        }

        public async Task<ProductoDto?> GetByIdAsync(long id)
        {
            var item = await _productoRepository.GetByIdAsync(id);
            return item == null ? null : MapToDto(item);
        }

        public async Task<IEnumerable<ProductoDto>> GetByCategoryAsync(long categoriaId)
        {
            var items = await _productoRepository.FindAsync(p => p.Categoria == categoriaId);
            return items.Select(MapToDto);
        }

        public async Task<ProductoDto?> UpdateAsync(long id, UpdateProductoDto dto)
        {
            var entity = await _productoRepository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.Nombre = dto.Nombre;
            entity.Descripcion = dto.Descripcion;
            entity.Price = dto.Price;
            entity.Descuento = dto.Descuento;
            entity.Sku = dto.Sku;
            entity.Lote = dto.Lote;
            entity.CostoUnitario = dto.CostoUnitario;
            entity.Iva = dto.Iva;
            entity.Categoria = dto.Categoria;

            await _productoRepository.UpdateAsync(entity);
            return MapToDto(entity);
        }

        private static ProductoDto MapToDto(Producto p)
        {
            return new ProductoDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Price = p.Price,
                Descuento = p.Descuento,
                Sku = p.Sku,
                Lote = p.Lote,
                CostoUnitario = p.CostoUnitario,
                Iva = p.Iva,
                Categoria = p.Categoria,
                CategoriaNombre = p.CategoriaNavigation?.Nombre
            };
        }
    }
}