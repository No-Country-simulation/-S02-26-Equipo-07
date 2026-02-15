using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs.Caballos;
using WebApi.Services;

namespace WebApi.Controllers
{

    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CaballoController : ControllerBase
    {
        private readonly ICaballoService _caballoService;
        private readonly ILogger<CaballoController> _logger;

        public CaballoController( ICaballoService caballoService, ILogger<CaballoController> logger)
        {
            _caballoService = caballoService;
            _logger = logger;
        }

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<CaballoDto>>> GetAllCaballos()
        {
            try
            {
                var caballos = await _caballoService.GetAllAsync();
                return Ok(caballos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving caballos");
                return StatusCode(500, new { message = "An error occurred while retrieving caballos" });
            }
        }

        //Falta verificar
    }
}
