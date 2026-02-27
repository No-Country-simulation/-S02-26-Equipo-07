using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using WebApi.DTOs.Auth;
using WebApi.Tests.IntegrationTests.Fixtures;
using Xunit;

namespace WebApi.Tests.IntegrationTests.Controllers;

public class VentasControllerTests : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client;
    private readonly WebApiFactory _factory;

    public VentasControllerTests(WebApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private async Task<string> GetUserTokenAsync()
    {
        var request = new RegisterRequest
        {
            Username = $"user_{Guid.NewGuid().ToString("N")[..8]}",
            Password = "password123",
            Role = "user"
        };
        var response = await _client.PostAsJsonAsync("/api/auth/register", request);
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return loginResponse!.Token;
    }

    [Fact]
    public async Task GetVenta_SinToken_Retorna401()
    {
        var response = await _client.GetAsync("/api/ventas/1");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetVentasUsuario_SinToken_Retorna401()
    {
        var response = await _client.GetAsync("/api/ventas/usuario");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetVentasUsuario_ConToken_Retorna200()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/ventas/usuario");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetVentaById_Inexistente_ConToken_Retorna404()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/ventas/999999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateVenta_SinDetalles_Retorna400()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var request = new
        {
            FechaVenta = DateTime.UtcNow,
            SubTotal = 0,
            Descuento = 0,
            IVA = 0,
            Total = 0,
            Detalles = new List<object>()
        };

        var response = await _client.PostAsJsonAsync("/api/ventas", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AnularVenta_Inexistente_RetornaRespuesta()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync("/api/ventas/999999");

        response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized);
    }
}
