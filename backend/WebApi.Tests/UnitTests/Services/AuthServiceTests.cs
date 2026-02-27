using FluentAssertions;
using Moq;
using WebApi.Configuration;
using WebApi.DTOs.Auth;
using WebApi.Models;
using WebApi.Repositories;
using WebApi.Services;
using Xunit;

namespace WebApi.Tests.UnitTests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly JwtSettings _jwtSettings;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _jwtSettings = new JwtSettings
        {
            SecretKey = "TestSecretKey_Minimum32Characters!",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpirationHours = 1
        };
        _authService = new AuthService(_userRepositoryMock.Object, _jwtSettings);
    }

    #region RegisterAsync

    [Fact]
    public async Task RegisterAsync_ConDatosValidos_RetornaLoginResponse()
    {
        var request = new RegisterRequest
        {
            Username = "testuser",
            Password = "password123",
            Role = "user"
        };

        _userRepositoryMock
            .Setup(x => x.UsernameExistsAsync(request.Username))
            .ReturnsAsync(false);

        _userRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => u);

        var result = await _authService.RegisterAsync(request);

        result.Should().NotBeNull();
        result.Username.Should().Be(request.Username);
        result.Role.Should().Be(request.Role);
        result.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task RegisterAsync_UsuarioExistente_LanzaExcepcion()
    {
        var request = new RegisterRequest
        {
            Username = "existinguser",
            Password = "password123"
        };

        _userRepositoryMock
            .Setup(x => x.UsernameExistsAsync(request.Username))
            .ReturnsAsync(true);

        var act = () => _authService.RegisterAsync(request);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Username already exists");
    }

    [Fact]
    public async Task RegisterAsync_RolInvalido_LanzaExcepcion()
    {
        var request = new RegisterRequest
        {
            Username = "newuser",
            Password = "password123",
            Role = "superadmin"
        };

        _userRepositoryMock
            .Setup(x => x.UsernameExistsAsync(request.Username))
            .ReturnsAsync(false);

        var act = () => _authService.RegisterAsync(request);
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Role must be 'admin' or 'user'");
    }

    [Fact]
    public async Task RegisterAsync_RolAdmin_RetornaLoginResponse()
    {
        var request = new RegisterRequest
        {
            Username = "adminuser",
            Password = "password123",
            Role = "admin"
        };

        _userRepositoryMock
            .Setup(x => x.UsernameExistsAsync(request.Username))
            .ReturnsAsync(false);

        _userRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => u);

        var result = await _authService.RegisterAsync(request);

        result.Should().NotBeNull();
        result.Role.Should().Be("admin");
    }

    [Fact]
    public async Task RegisterAsync_RolNull_DefaultUser()
    {
        var request = new RegisterRequest
        {
            Username = "defaultuser",
            Password = "password123",
            Role = null
        };

        _userRepositoryMock
            .Setup(x => x.UsernameExistsAsync(request.Username))
            .ReturnsAsync(false);

        _userRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => u);

        var result = await _authService.RegisterAsync(request);

        result.Should().NotBeNull();
        result.Role.Should().Be("user");
    }

    [Fact]
    public async Task RegisterAsync_ConPasswordValido_GeneraHash()
    {
        var request = new RegisterRequest
        {
            Username = "hashuser",
            Password = "password123",
            Role = "user"
        };

        User? capturedUser = null;
        _userRepositoryMock
            .Setup(x => x.UsernameExistsAsync(request.Username))
            .ReturnsAsync(false);

        _userRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<User>()))
            .Callback<User>(u => capturedUser = u)
            .ReturnsAsync((User u) => u);

        await _authService.RegisterAsync(request);

        capturedUser.Should().NotBeNull();
        capturedUser!.PasswordHash.Should().NotBeNullOrEmpty();
        capturedUser.PasswordHash.Should().NotBe(request.Password);
    }

    [Fact]
    public async Task RegisterAsync_UsuarioNuevo_GeneraTokenValido()
    {
        var request = new RegisterRequest
        {
            Username = "tokenuser",
            Password = "password123",
            Role = "user"
        };

        _userRepositoryMock
            .Setup(x => x.UsernameExistsAsync(request.Username))
            .ReturnsAsync(false);

        _userRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => u);

        var result = await _authService.RegisterAsync(request);

        result.Token.Should().NotBeNullOrEmpty();
        result.ExpiresAt.Should().BeAfter(DateTime.UtcNow);
    }

    #endregion

    #region LoginAsync

    [Fact]
    public async Task LoginAsync_CredencialesValidas_RetornaLoginResponse()
    {
        var request = new LoginRequest
        {
            Username = "testuser",
            Password = "password123"
        };

        var user = new User
        {
            Id = 1,
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = "user",
            Status = "habilitado"
        };

        _userRepositoryMock
            .Setup(x => x.GetByUsernameAsync(request.Username))
            .ReturnsAsync(user);

        var (response, error) = await _authService.LoginAsync(request);

        error.Should().BeNull();
        response.Should().NotBeNull();
        response.Username.Should().Be(request.Username);
        response.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task LoginAsync_UsuarioNoExiste_RetornaError()
    {
        var request = new LoginRequest
        {
            Username = "nonexistent",
            Password = "password123"
        };

        _userRepositoryMock
            .Setup(x => x.GetByUsernameAsync(request.Username))
            .ReturnsAsync((User?)null);

        var (response, error) = await _authService.LoginAsync(request);

        response.Should().BeNull();
        error.Should().Be("Usuario no registrado");
    }

    [Fact]
    public async Task LoginAsync_UsuarioDeshabilitado_RetornaError()
    {
        var request = new LoginRequest
        {
            Username = "testuser",
            Password = "password123"
        };

        var user = new User
        {
            Id = 1,
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = "user",
            Status = "deshabilitado"
        };

        _userRepositoryMock
            .Setup(x => x.GetByUsernameAsync(request.Username))
            .ReturnsAsync(user);

        var (response, error) = await _authService.LoginAsync(request);

        response.Should().BeNull();
        error.Should().Be("Usuario deshabilitado");
    }

    [Fact]
    public async Task LoginAsync_PasswordIncorrecto_RetornaError()
    {
        var request = new LoginRequest
        {
            Username = "testuser",
            Password = "wrongpassword"
        };

        var user = new User
        {
            Id = 1,
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword"),
            Role = "user",
            Status = "habilitado"
        };

        _userRepositoryMock
            .Setup(x => x.GetByUsernameAsync(request.Username))
            .ReturnsAsync(user);

        var (response, error) = await _authService.LoginAsync(request);

        response.Should().BeNull();
        error.Should().Be("Contraseña incorrecta");
    }

    [Fact]
    public async Task LoginAsync_UsuarioHabilitado_RetornaToken()
    {
        var request = new LoginRequest
        {
            Username = "enableduser",
            Password = "password123"
        };

        var user = new User
        {
            Id = 1,
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = "user",
            Status = "habilitado"
        };

        _userRepositoryMock
            .Setup(x => x.GetByUsernameAsync(request.Username))
            .ReturnsAsync(user);

        var (response, error) = await _authService.LoginAsync(request);

        response.Should().NotBeNull();
        response!.Token.Should().NotBeNullOrEmpty();
        response.ExpiresAt.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task LoginAsync_RolAdmin_RetornaAdminRole()
    {
        var request = new LoginRequest
        {
            Username = "adminuser",
            Password = "password123"
        };

        var user = new User
        {
            Id = 1,
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = "admin",
            Status = "habilitado"
        };

        _userRepositoryMock
            .Setup(x => x.GetByUsernameAsync(request.Username))
            .ReturnsAsync(user);

        var (response, error) = await _authService.LoginAsync(request);

        response.Should().NotBeNull();
        response!.Role.Should().Be("admin");
    }

    #endregion

    #region GenerateJwtToken

    [Fact]
    public void GenerateJwtToken_ConParametrosValidos_RetornaToken()
    {
        var username = "testuser";
        var role = "admin";
        var userId = 1L;

        var token = _authService.GenerateJwtToken(username, role, userId);

        token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GenerateJwtToken_DiferentesLlamadas_GeneraTokensDiferentes()
    {
        var token1 = _authService.GenerateJwtToken("user1", "admin", 1L);
        var token2 = _authService.GenerateJwtToken("user2", "user", 2L);

        token1.Should().NotBe(token2);
    }

    #endregion
}
