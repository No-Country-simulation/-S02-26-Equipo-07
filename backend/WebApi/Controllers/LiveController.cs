using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("")]                          
public class HealthController : ControllerBase
{
    [HttpGet("live")]                
    public IActionResult Check()
    {
        return Ok("OK");
    }

}