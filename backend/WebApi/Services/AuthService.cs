using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApi.Configuration;
using WebApi.DTOs.Auth;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;

    public AuthService(IUserRepository userRepository, JwtSettings jwtSettings)
    {
        _userRepository = userRepository;
        _jwtSettings = jwtSettings;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);
        
        if (user == null)
            return null;

        if (user.Status != "habilitado")
            return null;

        if (!PasswordHasher.VerifyPassword(request.Password, user.PasswordHash!))
            return null;

        var token = GenerateJwtToken(user.Username, user.Role, user.Id);
        var expiresAt = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours);

        return new LoginResponse
        {
            Token = token,
            Username = user.Username,
            Role = user.Role,
            ExpiresAt = expiresAt
        };
    }

    public async Task<LoginResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepository.UsernameExistsAsync(request.Username))
            throw new InvalidOperationException("Username already exists");

        var hashedPassword = PasswordHasher.HashPassword(request.Password);

        var user = new User
        {
            Username = request.Username,
            PasswordHash = hashedPassword,
            Role = request.Role,
            Status = "habilitado",
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };

        await _userRepository.AddAsync(user);

        var token = GenerateJwtToken(user.Username, user.Role, user.Id);
        var expiresAt = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours);

        return new LoginResponse
        {
            Token = token,
            Username = user.Username,
            Role = user.Role,
            ExpiresAt = expiresAt
        };
    }

    public string GenerateJwtToken(string username, string role, long userId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
