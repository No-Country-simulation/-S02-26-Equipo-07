using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using WebApi.DTOs.Auth;
using WebApi.Tests.IntegrationTests.Fixtures;
using Xunit;

namespace WebApi.Tests.IntegrationTests.Controllers;

public class AuthControllerTests : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client;
    private readonly WebApiFactory _factory;

    public AuthControllerTests(WebApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private string GenerateUniqueUsername(string prefix)
    {
        return $"{prefix}_{Guid.NewGuid().ToString("N")[..8]}";
    }

    [Fact]
    public async Task Register_ConDatosValidos_Retorna200()
    {
        var request = new RegisterRequest
        {
            Username = GenerateUniqueUsername("testuser"),
            Password = "password123",
            Role = "user"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<LoginResponse>();
        content.Should().NotBeNull();
        content!.Token.Should().NotBeNullOrEmpty();
        content.Username.Should().Be(request.Username);
    }

    [Fact]
    public async Task Register_UsuarioDuplicado_Retorna400()
    {
        var username = GenerateUniqueUsername("duplicate");
        var request = new RegisterRequest
        {
            Username = username,
            Password = "password123",
            Role = "user"
        };

        await _client.PostAsJsonAsync("/api/auth/register", request);

        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_RolInvalido_RetornaError()
    {
        var request = new RegisterRequest
        {
            Username = GenerateUniqueUsername("invalidrole"),
            Password = "password123",
            Role = "superadmin"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task Register_RolAdmin_Retorna200()
    {
        var request = new RegisterRequest
        {
            Username = GenerateUniqueUsername("admin"),
            Password = "password123",
            Role = "admin"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<LoginResponse>();
        content.Should().NotBeNull();
        content!.Role.Should().Be("admin");
    }

    [Fact]
    public async Task Login_CredencialesValidas_Retorna200()
    {
        var username = GenerateUniqueUsername("logintest");
        var password = "password123";
        
        var registerRequest = new RegisterRequest
        {
            Username = username,
            Password = password,
            Role = "user"
        };
        
        await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

        var loginRequest = new LoginRequest
        {
            Username = username,
            Password = password
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<LoginResponse>();
        content.Should().NotBeNull();
        content!.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_UsuarioNoExiste_RetornaUnauthorized()
    {
        var request = new LoginRequest
        {
            Username = "nonexistent_user",
            Password = "password123"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_PasswordIncorrecto_RetornaUnauthorized()
    {
        var username = GenerateUniqueUsername("wrongpass");
        
        var registerRequest = new RegisterRequest
        {
            Username = username,
            Password = "correctpassword",
            Role = "user"
        };
        
        await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

        var loginRequest = new LoginRequest
        {
            Username = username,
            Password = "wrongpassword"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Register_TokenValido_RetornaToken()
    {
        var username = GenerateUniqueUsername("tokenuser");
        var request = new RegisterRequest
        {
            Username = username,
            Password = "password123",
            Role = "user"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<LoginResponse>();
        content.Should().NotBeNull();
        content!.Token.Should().NotBeNullOrEmpty();
        content.ExpiresAt.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task Register_MultipleUsers_DifferentTokens()
    {
        var username1 = GenerateUniqueUsername("user1");
        var username2 = GenerateUniqueUsername("user2");
        
        var registerRequest1 = new RegisterRequest { Username = username1, Password = "password123", Role = "user" };
        var registerRequest2 = new RegisterRequest { Username = username2, Password = "password123", Role = "user" };

        var response1 = await _client.PostAsJsonAsync("/api/auth/register", registerRequest1);
        var response2 = await _client.PostAsJsonAsync("/api/auth/register", registerRequest2);

        var loginResponse1 = await response1.Content.ReadFromJsonAsync<LoginResponse>();
        var loginResponse2 = await response2.Content.ReadFromJsonAsync<LoginResponse>();

        loginResponse1!.Token.Should().NotBe(loginResponse2!.Token);
    }

    [Fact]
    public async Task Register_UsernameVacio_RetornaBadRequest()
    {
        var request = new RegisterRequest
        {
            Username = "",
            Password = "password123",
            Role = "user"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_PasswordVacio_RetornaBadRequest()
    {
        var request = new RegisterRequest
        {
            Username = GenerateUniqueUsername("emptypass"),
            Password = "",
            Role = "user"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_PasswordMuyCorto_RetornaBadRequest()
    {
        var request = new RegisterRequest
        {
            Username = GenerateUniqueUsername("shortpass"),
            Password = "123",
            Role = "user"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_UsuarioYPasswordVacios_RetornaBadRequest()
    {
        var request = new LoginRequest
        {
            Username = "",
            Password = ""
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
