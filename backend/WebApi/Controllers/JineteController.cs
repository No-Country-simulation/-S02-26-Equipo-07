using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs.Jinetes;
using WebApi.Services;

namespace WebApi.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class JineteController : ControllerBase
    {
        private readonly IJineteService _jineteService;
        private readonly ILogger<JineteController> _logger;

        public JineteController(IJineteService jineteService, ILogger<JineteController> logger)
        {
            _jineteService = jineteService;
            _logger = logger;
        }


        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<JineteDto>>> GetAllJinetes()
        {
            try
            {
                var jinetes = await _jineteService.GetAllAsync();
                return Ok(jinetes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving jinetes");
                return StatusCode(500, new { message = "An error occurred while retrieving jinetes" });
            }
        }

        //Falta verificar
    }
}
