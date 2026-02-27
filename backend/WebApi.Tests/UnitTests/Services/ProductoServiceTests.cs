using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using WebApi.DTOs.Productos;
using WebApi.Models;
using WebApi.Repositories;
using WebApi.Services;
using Xunit;

namespace WebApi.Tests.UnitTests.Services;

public class ProductoServiceTests
{
    private readonly Mock<IProductoRepository> _productoRepositoryMock;
    private readonly ProductoService _productoService;

    public ProductoServiceTests()
    {
        _productoRepositoryMock = new Mock<IProductoRepository>();
        _productoService = new ProductoService(_productoRepositoryMock.Object);
    }

    #region CreateAsync

    [Fact]
    public async Task CreateAsync_ConDatosValidos_RetornaProductoDto()
    {
        var dto = new CreateProductoDto
        {
            Nombre = "Producto Test",
            Descripcion = "Descripcion Test",
            Price = 100.00m,
            Descuento = 10.00m,
            Sku = "SKU-001",
            Lote = "LOTE-001",
            CostoUnitario = 50.00m,
            Iva = 21,
            Categoria = 1
        };

        _productoRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Producto>()))
            .ReturnsAsync((Producto p) => { p.Id = 1; return p; });

        var result = await _productoService.CreateAsync(dto);

        result.Should().NotBeNull();
        result.Nombre.Should().Be(dto.Nombre);
        result.Price.Should().Be(dto.Price);
        result.Sku.Should().Be(dto.Sku);
    }

    [Fact]
    public async Task CreateAsync_ConDatosValidos_GuardaEnRepositorio()
    {
        var dto = new CreateProductoDto
        {
            Nombre = "Producto Test",
            Descripcion = "Descripcion Test",
            Price = 100.00m,
            Categoria = 1
        };

        Producto? capturedProducto = null;
        _productoRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Producto>()))
            .Callback<Producto>(p => capturedProducto = p)
            .ReturnsAsync((Producto p) => p);

        await _productoService.CreateAsync(dto);

        capturedProducto.Should().NotBeNull();
        capturedProducto!.Nombre.Should().Be(dto.Nombre);
    }

    #endregion

    #region GetAllAsync

    [Fact]
    public async Task GetAllAsync_ConProductosExistentes_RetornaLista()
    {
        var productos = new List<Producto>
        {
            new Producto { Id = 1, Nombre = "Producto 1", Price = 100, Categoria = 1 },
            new Producto { Id = 2, Nombre = "Producto 2", Price = 200, Categoria = 1 }
        };

        _productoRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(productos);

        var result = await _productoService.GetAllAsync();

        result.Should().NotBeNull();
        result.Count().Should().Be(2);
    }

    [Fact]
    public async Task GetAllAsync_SinProductos_RetornaListaVacia()
    {
        _productoRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Producto>());

        var result = await _productoService.GetAllAsync();

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    #endregion

    #region GetByIdAsync

    [Fact]
    public async Task GetByIdAsync_ProductoExistente_RetornaProductoDto()
    {
        var producto = new Producto
        {
            Id = 1,
            Nombre = "Producto Test",
            Price = 100.00m,
            Categoria = 1
        };

        _productoRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(producto);

        var result = await _productoService.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Nombre.Should().Be("Producto Test");
    }

    [Fact]
    public async Task GetByIdAsync_ProductoNoExiste_RetornaNull()
    {
        _productoRepositoryMock
            .Setup(x => x.GetByIdAsync(999))
            .ReturnsAsync((Producto?)null);

        var result = await _productoService.GetByIdAsync(999);

        result.Should().BeNull();
    }

    #endregion

    #region GetByCategoryAsync

    [Fact]
    public async Task GetByCategoryAsync_CategoriaConProductos_RetornaLista()
    {
        var productos = new List<Producto>
        {
            new Producto { Id = 1, Nombre = "Producto 1", Categoria = 1 },
            new Producto { Id = 2, Nombre = "Producto 2", Categoria = 1 }
        };

        _productoRepositoryMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Producto, bool>>>()))
            .ReturnsAsync(productos);

        var result = await _productoService.GetByCategoryAsync(1);

        result.Should().NotBeNull();
        result.Count().Should().Be(2);
    }

    #endregion

    #region UpdateAsync

    [Fact]
    public async Task UpdateAsync_ProductoExistente_RetornaProductoActualizado()
    {
        var existingProducto = new Producto
        {
            Id = 1,
            Nombre = "Producto Original",
            Price = 100.00m,
            Categoria = 1
        };

        var updateDto = new UpdateProductoDto
        {
            Nombre = "Producto Actualizado",
            Descripcion = "Nueva descripcion",
            Price = 150.00m,
            Descuento = 5.00m,
            Sku = "SKU-NEW",
            Lote = "LOTE-NEW",
            CostoUnitario = 75.00m,
            Iva = 21,
            Categoria = 2
        };

        _productoRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(existingProducto);

        _productoRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Producto>()))
            .Returns(Task.CompletedTask);

        var result = await _productoService.UpdateAsync(1, updateDto);

        result.Should().NotBeNull();
        result.Nombre.Should().Be(updateDto.Nombre);
        result.Price.Should().Be(updateDto.Price);
    }

    [Fact]
    public async Task UpdateAsync_ProductoNoExiste_RetornaNull()
    {
        var updateDto = new UpdateProductoDto
        {
            Nombre = "Producto Test",
            Price = 100.00m,
            Categoria = 1
        };

        _productoRepositoryMock
            .Setup(x => x.GetByIdAsync(999))
            .ReturnsAsync((Producto?)null);

        var result = await _productoService.UpdateAsync(999, updateDto);

        result.Should().BeNull();
    }

    #endregion

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_ProductoExistente_RetornaTrue()
    {
        var producto = new Producto
        {
            Id = 1,
            Nombre = "Producto Test"
        };

        _productoRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(producto);

        _productoRepositoryMock
            .Setup(x => x.DeleteAsync(It.IsAny<Producto>()))
            .Returns(Task.CompletedTask);

        var result = await _productoService.DeleteAsync(1);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ProductoNoExiste_RetornaFalse()
    {
        _productoRepositoryMock
            .Setup(x => x.GetByIdAsync(999))
            .ReturnsAsync((Producto?)null);

        var result = await _productoService.DeleteAsync(999);

        result.Should().BeFalse();
    }

    #endregion
}
