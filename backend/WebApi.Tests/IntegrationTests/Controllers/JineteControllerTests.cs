using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using WebApi.Tests.IntegrationTests.Fixtures;
using Xunit;

namespace WebApi.Tests.IntegrationTests.Controllers;

public class JineteControllerTests : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client;
    private readonly WebApiFactory _factory;

    public JineteControllerTests(WebApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllJinetes_Retorna200()
    {
        var response = await _client.GetAsync("/api/jinete");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllJinetes_RetornaLista()
    {
        var response = await _client.GetAsync("/api/jinete");

        var jinetes = await response.Content.ReadFromJsonAsync<List<object>>();
        jinetes.Should().NotBeNull();
    }
}
