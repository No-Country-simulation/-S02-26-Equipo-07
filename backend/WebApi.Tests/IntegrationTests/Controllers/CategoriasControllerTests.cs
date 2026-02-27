using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using WebApi.DTOs.Auth;
using WebApi.DTOs.Categorias;
using WebApi.Tests.IntegrationTests.Fixtures;
using Xunit;

namespace WebApi.Tests.IntegrationTests.Controllers;

public class CategoriasControllerTests : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client;
    private readonly WebApiFactory _factory;

    public CategoriasControllerTests(WebApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private async Task<string> GetAdminTokenAsync()
    {
        var request = new RegisterRequest
        {
            Username = $"admin_{Guid.NewGuid().ToString("N")[..8]}",
            Password = "password123",
            Role = "admin"
        };
        var response = await _client.PostAsJsonAsync("/api/auth/register", request);
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return loginResponse!.Token;
    }

    [Fact]
    public async Task GetAllCategorias_SinToken_Retorna401()
    {
        var response = await _client.GetAsync("/api/categorias");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAllCategorias_ConToken_Retorna200()
    {
        var token = await GetAdminTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/categorias");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetCategoriaById_Inexistente_Retorna404()
    {
        var token = await GetAdminTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/categorias/999999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetCategoriaChildren_Retorna200()
    {
        var token = await GetAdminTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/categorias/1/children");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateCategoria_ConToken_Retorna201()
    {
        var token = await GetAdminTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var request = new CreateCategoriaDto
        {
            Nombre = $"TestCategoria_{Guid.NewGuid():N}".Substring(0, 20),
            Descripcion = "Test Description"
        };

        var response = await _client.PostAsJsonAsync("/api/categorias", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task UpdateCategoria_Inexistente_Retorna404()
    {
        var token = await GetAdminTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var request = new UpdateCategoriaDto
        {
            Nombre = "Updated Category",
            Descripcion = "Updated Description"
        };

        var response = await _client.PutAsJsonAsync("/api/categorias/999999", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteCategoria_Inexistente_Retorna404()
    {
        var token = await GetAdminTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync("/api/categorias/999999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
