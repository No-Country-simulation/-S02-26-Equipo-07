using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Auth;

public class RegisterRequest
{
    [Required]
    [MaxLength(25)]
    public string Username { get; set; } = null!;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = null!;

    [MaxLength(10)]
    public string Role { get; set; } = "user";
}
