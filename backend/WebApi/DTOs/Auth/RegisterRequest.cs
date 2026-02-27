using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Auth;

public class RegisterRequest
{
    [Required]
    [MaxLength(25)]
    public string Username { get; set; } = null!;

    [Required]
    [MinLength(6)] // La contrase�a debe tener al menos 6 caracteres
    public string Password { get; set; } = null!;

    // Email es opcional - no se guarda en DB
    public string? Email { get; set; }

    [MaxLength(10)]
    public string Role { get; set; } = "user";
}