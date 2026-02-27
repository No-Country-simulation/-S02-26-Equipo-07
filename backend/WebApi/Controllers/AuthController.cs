using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs.Auth;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var result = await _authService.RegisterAsync(request);
            _logger.LogInformation("Usuario {Username} registrado exitosamente", request.Username);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Intento de registro fallido para usuario {Username}", request.Username);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante el registro");
            return StatusCode(500, new { message = "Error al registrar usuario" });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var (response, error) = await _authService.LoginAsync(request);
            
            if (error != null)
            {
                _logger.LogWarning("Intento de login fallido para usuario {Username}: {Error}", request.Username, error);
                return Unauthorized(new { message = error });
            }

            _logger.LogInformation("Usuario {Username} inició sesión", request.Username);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante el login");
            return StatusCode(500, new { message = "Error al iniciar sesión" });
        }
    }
}
