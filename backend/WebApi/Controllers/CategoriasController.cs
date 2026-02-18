using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs.Categorias;
using WebApi.Services;

namespace WebApi.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(ICategoriaService categoriaService, ILogger<CategoriasController> logger)
        {
            _categoriaService = categoriaService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCategoriaDto>>> GetAll()
        {
            try
            {
                var items = await _categoriaService.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categorias");
                return StatusCode(500, new { message = "An error occurred while retrieving categorias" });
            }
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<GetCategoriaDto>> GetById(long id)
        {
            try
            {
                var item = await _categoriaService.GetByIdAsync(id);
                if (item == null) return NotFound();
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categoria");
                return StatusCode(500, new { message = "An error occurred while retrieving categoria" });
            }
        }

        [HttpGet("{id:long}/children")]
        public async Task<ActionResult<IEnumerable<GetCategoriaDto>>> GetChildren(long id)
        {
            try
            {
                var items = await _categoriaService.GetAllChildrenById(id);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categoria children");
                return StatusCode(500, new { message = "An error occurred while retrieving categoria children" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<GetCategoriaDto>> Create([FromBody] CreateCategoriaDto dto)
        {
            try
            {
                var created = await _categoriaService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating categoria");
                return StatusCode(500, new { message = "An error occurred while creating categoria" });
            }
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<GetCategoriaDto>> Update(long id, [FromBody] UpdateCategoriaDto dto)
        {
            try
            {
                var updated = await _categoriaService.UpdateAsync(id, dto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating categoria");
                return StatusCode(500, new { message = "An error occurred while updating categoria" });
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                var deleted = await _categoriaService.DeleteAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting categoria");
                return StatusCode(500, new { message = "An error occurred while deleting categoria" });
            }
        }
    }
}
