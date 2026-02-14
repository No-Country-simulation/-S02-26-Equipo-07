using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Auth;

public class LoginRequest
{
    [Required]
    [MaxLength(25)]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
