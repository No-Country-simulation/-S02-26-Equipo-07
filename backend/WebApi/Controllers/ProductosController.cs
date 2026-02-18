using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs.Productos;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;
        private readonly ILogger<ProductosController> _logger;

        public ProductosController(IProductoService productoService, ILogger<ProductosController> logger)
        {
            _productoService = productoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetAll()
        {
            try
            {
                var items = await _productoService.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving productos");
                return StatusCode(500, new { message = "An error occurred while retrieving productos" });
            }
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ProductoDto>> GetById(long id)
        {
            var item = await _productoService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpGet("category/{categoriaId:long}")]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetByCategory(long categoriaId)
        {
            var items = await _productoService.GetByCategoryAsync(categoriaId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<ProductoDto>> Create([FromBody] CreateProductoDto dto)
        {
            var created = await _productoService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<ProductoDto>> Update(long id, [FromBody] UpdateProductoDto dto)
        {
            var updated = await _productoService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> Delete(long id)
        {
            var deleted = await _productoService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}