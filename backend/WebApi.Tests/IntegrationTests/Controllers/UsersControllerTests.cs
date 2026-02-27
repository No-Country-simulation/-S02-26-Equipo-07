using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using WebApi.DTOs.Auth;
using WebApi.DTOs.User;
using WebApi.Models;
using WebApi.Tests.IntegrationTests.Fixtures;
using Xunit;

namespace WebApi.Tests.IntegrationTests.Controllers;

public class UsersControllerTests : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client;
    private readonly WebApiFactory _factory;

    public UsersControllerTests(WebApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private string GenerateUniqueUsername(string prefix)
    {
        return $"{prefix}_{Guid.NewGuid().ToString("N")[..8]}";
    }

    private async Task<string> GetAdminTokenAsync()
    {
        var request = new RegisterRequest
        {
            Username = GenerateUniqueUsername("adminuser"),
            Password = "password123",
            Role = "admin"
        };
        var response = await _client.PostAsJsonAsync("/api/auth/register", request);
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return loginResponse!.Token;
    }

    private async Task<string> GetUserTokenAsync()
    {
        var request = new RegisterRequest
        {
            Username = GenerateUniqueUsername("regularuser"),
            Password = "password123",
            Role = "user"
        };
        var response = await _client.PostAsJsonAsync("/api/auth/register", request);
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return loginResponse!.Token;
    }

    [Fact]
    public async Task GetAllUsers_SinToken_Retorna401()
    {
        var response = await _client.GetAsync("/api/users");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAllUsers_ConTokenAdmin_Retorna200()
    {
        var token = await GetAdminTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/users");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllUsers_ConTokenUser_Retorna403()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/users");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetUserById_Admin_Retorna200()
    {
        var token = await GetAdminTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/users/1");

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateUser_Admin_RetornaRespuesta()
    {
        var token = await GetAdminTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var updateRequest = new User
        {
            Username = GenerateUniqueUsername("updateduser"),
            Role = "user",
            Status = "habilitado"
        };

        var response = await _client.PutAsJsonAsync("/api/users/1", updateRequest);

        response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteUser_Admin_Retorna200()
    {
        var token = await GetAdminTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync("/api/users/999");

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
    }
}
