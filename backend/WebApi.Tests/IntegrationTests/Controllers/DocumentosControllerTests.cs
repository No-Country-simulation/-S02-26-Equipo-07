using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using WebApi.DTOs.Auth;
using WebApi.Tests.IntegrationTests.Fixtures;
using Xunit;

namespace WebApi.Tests.IntegrationTests.Controllers;

public class DocumentosControllerTests : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client;
    private readonly WebApiFactory _factory;

    public DocumentosControllerTests(WebApiFactory factory)
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
    public async Task GetFactura_SinToken_Retorna401()
    {
        var response = await _client.GetAsync("/api/documentos/facturas/1");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetFactura_Inexistente_ConToken_Retorna404()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/documentos/facturas/999999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetFacturasPorVenta_ConToken_Retorna200()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/documentos/facturas/venta/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task EmitirFactura_DatosInvalidos_Retorna400()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var request = new
        {
            VentaId = 0,
            TipoFactura = "A"
        };

        var response = await _client.PostAsJsonAsync("/api/documentos/facturas", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AnularFactura_Inexistente_ConToken_Retorna400()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync("/api/documentos/facturas/999999");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task EmitirNotaCredito_DatosInvalidos_Retorna400()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var request = new
        {
            FacturaId = 0,
            TipoNotaCredito = "A"
        };

        var response = await _client.PostAsJsonAsync("/api/documentos/notas-credito", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task EmitirNotaDebito_DatosInvalidos_Retorna400()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var request = new
        {
            FacturaId = 0,
            TipoNotaDebito = "A"
        };

        var response = await _client.PostAsJsonAsync("/api/documentos/notas-debito", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetResumenDocumentos_FechasInvalidas_Retorna400()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var desde = DateTime.UtcNow.AddDays(10);
        var hasta = DateTime.UtcNow;

        var response = await _client.GetAsync($"/api/documentos/reportes/resumen?desde={desde:O}&hasta={hasta:O}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetNotaCredito_Inexistente_Retorna404()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/documentos/notas-credito/999999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetNotaDebito_Inexistente_Retorna404()
    {
        var token = await GetUserTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/documentos/notas-debito/999999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
