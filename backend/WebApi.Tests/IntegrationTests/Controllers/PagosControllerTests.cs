using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using WebApi.DTOs.Auth;
using WebApi.Tests.IntegrationTests.Fixtures;
using Xunit;

namespace WebApi.Tests.IntegrationTests.Controllers;

public class PagosControllerTests : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client;
    private readonly WebApiFactory _factory;

    public PagosControllerTests(WebApiFactory factory)
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
    public async Task GetPago_SinToken_Retorna401()
    {
        var response = await _client.GetAsync("/api/pagos/1");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetPagosPorVenta_SinToken_Retorna401()
    {
        var response = await _client.GetAsync("/api/pagos/venta/1");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetPagosPorVenta_ConToken_Retorna200()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/pagos/venta/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetEstadoPago_ConToken_Retorna200()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/pagos/venta/1/estado");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RegistrarPago_DatosInvalidos_Retorna400()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var request = new
        {
            VentaId = 0,
            Monto = -100,
            MetodoPago = ""
        };

        var response = await _client.PostAsJsonAsync("/api/pagos", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetResumenPagos_FechasInvalidas_Retorna400()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var desde = DateTime.UtcNow.AddDays(10);
        var hasta = DateTime.UtcNow;

        var response = await _client.GetAsync($"/api/pagos/reportes/resumen?desde={desde:O}&hasta={hasta:O}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
