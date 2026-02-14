using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Auth;

public class RegisterRequest
{
    [Required]
    [MaxLength(25)]
    public string Username { get; set; } = null!;

    [Required]
    [MinLength(6)] // La contraseña debe tener al menos 6 caracteres
    public string Password { get; set; } = null!;

    // AGREGA ESTO:
    [Required]
    [EmailAddress] // Esto valida que tenga formato de correo (ej: nombre@dominio.com)
    public string Email { get; set; } = null!;

    [MaxLength(10)]
    public string Role { get; set; } = "user";
}