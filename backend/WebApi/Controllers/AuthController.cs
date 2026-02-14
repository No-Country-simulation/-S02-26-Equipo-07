using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs.Auth;
using WebApi.DTOs.User;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // Simulación de base de datos en memoria
    // Usamos RegisterRequest porque es el DTO que ya tienes definido
    private static List<UserDto> _users = new List<UserDto>();
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Registro de un nuevo usuario
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<string>> Register([FromBody] RegisterRequest request)
    {
        // 1. Validamos si el usuario ya existe en la lista de UserDto
        if (_users.Any(u => u.Username == request.Username))
            return BadRequest(new { message = "El usuario ya existe" });

        // 2. Aquí es donde "mapeamos" los datos: 
        // Pasamos lo que llega de la web (RegisterRequest) al formato de base de datos (UserDto)
        var newUser = new UserDto
        {
            Username = request.Username,
            Password = request.Password,
            Email = request.Email,
            Role = request.Role ?? "user"
        };

        _users.Add(newUser);
        return Ok(new { message = "Usuario registrado con éxito" });
    }

    /// <summary>
    /// Login con usuario y contraseña
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<object>> Login([FromBody] LoginRequest request)
    {
        try
        {
            // Buscamos al usuario en la lista
            var user = _users.FirstOrDefault(u =>
                u.Username == request.Username &&
                u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Usuario o contraseña incorrectos" });
            }

            _logger.LogInformation("Usuario {Username} inició sesión", request.Username);

            // Retornamos una respuesta de éxito
            return Ok(new
            {
                message = $"¡Bienvenido, {user.Username}!",
                token = "token-simulado-12345" // Aquí luego irá el JWT real
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante el login");
            return StatusCode(500, new { message = "Ocurrió un error en el servidor" });
        }
    }
}