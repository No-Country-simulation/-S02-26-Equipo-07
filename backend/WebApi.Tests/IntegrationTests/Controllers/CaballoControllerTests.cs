using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using WebApi.Tests.IntegrationTests.Fixtures;
using Xunit;

namespace WebApi.Tests.IntegrationTests.Controllers;

public class CaballoControllerTests : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client;
    private readonly WebApiFactory _factory;

    public CaballoControllerTests(WebApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllCaballos_Retorna200()
    {
        var response = await _client.GetAsync("/api/caballo");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllCaballos_RetornaLista()
    {
        var response = await _client.GetAsync("/api/caballo");

        var caballos = await response.Content.ReadFromJsonAsync<List<object>>();
        caballos.Should().NotBeNull();
    }
}
