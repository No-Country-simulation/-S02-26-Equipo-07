using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using WebApi.DTOs.Productos;
using WebApi.Tests.IntegrationTests.Fixtures;
using Xunit;

namespace WebApi.Tests.IntegrationTests.Controllers;

public class ProductosControllerTests : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client;
    private readonly WebApiFactory _factory;

    public ProductosControllerTests(WebApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllProducts_Retorna200()
    {
        var response = await _client.GetAsync("/api/productos");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllProducts_RetornaLista()
    {
        var response = await _client.GetAsync("/api/productos");

        var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductoDto>>();
        products.Should().NotBeNull();
    }

    [Fact]
    public async Task GetProductById_Inexistente_Retorna404()
    {
        var response = await _client.GetAsync("/api/productos/999999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetProductsByCategory_Retorna200()
    {
        var response = await _client.GetAsync("/api/productos/category/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateProduct_ConDatos_Retorna201()
    {
        var request = new CreateProductoDto
        {
            Nombre = $"TestProd_{Guid.NewGuid():N}".Substring(0, 15),
            Descripcion = "Test Description",
            Price = 100.00m,
            Descuento = 10.00m,
            Sku = $"SKU-{Guid.NewGuid():N}".Substring(0, 10),
            Lote = "LOTE-001",
            CostoUnitario = 50.00m,
            Iva = 21,
            Categoria = 1
        };

        var response = await _client.PostAsJsonAsync("/api/productos", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task UpdateProduct_Inexistente_Retorna404()
    {
        var request = new UpdateProductoDto
        {
            Nombre = "Updated Product",
            Price = 150.00m,
            Categoria = 1
        };

        var response = await _client.PutAsJsonAsync("/api/productos/999999", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteProduct_Inexistente_Retorna404()
    {
        var response = await _client.DeleteAsync("/api/productos/999999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
