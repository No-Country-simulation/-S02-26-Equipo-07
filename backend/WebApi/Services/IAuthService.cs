using WebApi.DTOs.Auth;

namespace WebApi.Services;

public interface IAuthService
{
    Task<(LoginResponse? Response, string? Error)> LoginAsync(LoginRequest request);
    Task<LoginResponse> RegisterAsync(RegisterRequest request);
    string GenerateJwtToken(string username, string role, long userId);
}
