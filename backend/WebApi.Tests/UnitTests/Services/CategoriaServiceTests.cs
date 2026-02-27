using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using WebApi.DTOs.Categorias;
using WebApi.Models;
using WebApi.Repositories;
using WebApi.Services;
using Xunit;

namespace WebApi.Tests.UnitTests.Services;

public class CategoriaServiceTests
{
    private readonly Mock<ICategoriaRepository> _categoriaRepositoryMock;
    private readonly CategoriaService _categoriaService;

    public CategoriaServiceTests()
    {
        _categoriaRepositoryMock = new Mock<ICategoriaRepository>();
        _categoriaService = new CategoriaService(_categoriaRepositoryMock.Object);
    }

    #region CreateAsync

    [Fact]
    public async Task CreateAsync_ConDatosValidos_RetornaGetCategoriaDto()
    {
        var dto = new CreateCategoriaDto
        {
            Nombre = "Electronica",
            Descripcion = "Productos electronicos",
            CategoriaPadre = null
        };

        _categoriaRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Categorium>()))
            .ReturnsAsync((Categorium c) => { c.Id = 1; return c; });

        var result = await _categoriaService.CreateAsync(dto);

        result.Should().NotBeNull();
        result.Nombre.Should().Be(dto.Nombre);
        result.Descripcion.Should().Be(dto.Descripcion);
    }

    [Fact]
    public async Task CreateAsync_ConCategoriaPadre_RetornaCategoriaConPadre()
    {
        var dto = new CreateCategoriaDto
        {
            Nombre = "Computadoras",
            Descripcion = "Equipos de computo",
            CategoriaPadre = 1
        };

        _categoriaRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Categorium>()))
            .ReturnsAsync((Categorium c) => c);

        var result = await _categoriaService.CreateAsync(dto);

        result.Should().NotBeNull();
        result.CategoriaPadre.Should().Be(1);
    }

    #endregion

    #region GetAllAsync

    [Fact]
    public async Task GetAllAsync_ConCategoriasExistentes_RetornaLista()
    {
        var categorias = new List<Categorium>
        {
            new Categorium { Id = 1, Nombre = "Electronica", Descripcion = "Productos electronicos" },
            new Categorium { Id = 2, Nombre = "Ropa", Descripcion = "Prendas de ropa" }
        };

        _categoriaRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(categorias);

        var result = await _categoriaService.GetAllAsync();

        result.Should().NotBeNull();
        result.Count().Should().Be(2);
    }

    [Fact]
    public async Task GetAllAsync_SinCategorias_RetornaListaVacia()
    {
        _categoriaRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Categorium>());

        var result = await _categoriaService.GetAllAsync();

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    #endregion

    #region GetByIdAsync

    [Fact]
    public async Task GetByIdAsync_CategoriaExistente_RetornaGetCategoriaDto()
    {
        var categoria = new Categorium
        {
            Id = 1,
            Nombre = "Electronica",
            Descripcion = "Productos electronicos"
        };

        _categoriaRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(categoria);

        var result = await _categoriaService.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Nombre.Should().Be("Electronica");
    }

    [Fact]
    public async Task GetByIdAsync_CategoriaNoExiste_RetornaNull()
    {
        _categoriaRepositoryMock
            .Setup(x => x.GetByIdAsync(999))
            .ReturnsAsync((Categorium?)null);

        var result = await _categoriaService.GetByIdAsync(999);

        result.Should().BeNull();
    }

    #endregion

    #region GetAllChildrenById

    [Fact]
    public async Task GetAllChildrenById_ConHijos_RetornaLista()
    {
        var hijos = new List<Categorium>
        {
            new Categorium { Id = 2, Nombre = "Computadoras", CategoriaPadre = 1 },
            new Categorium { Id = 3, Nombre = "Celulares", CategoriaPadre = 1 }
        };

        _categoriaRepositoryMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Categorium, bool>>>()))
            .ReturnsAsync(hijos);

        var result = await _categoriaService.GetAllChildrenById(1);

        result.Should().NotBeNull();
        result.Count().Should().Be(2);
    }

    [Fact]
    public async Task GetAllChildrenById_SinHijos_RetornaListaVacia()
    {
        _categoriaRepositoryMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Categorium, bool>>>()))
            .ReturnsAsync(new List<Categorium>());

        var result = await _categoriaService.GetAllChildrenById(1);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    #endregion

    #region UpdateAsync

    [Fact]
    public async Task UpdateAsync_CategoriaExistente_RetornaCategoriaActualizada()
    {
        var existingCategoria = new Categorium
        {
            Id = 1,
            Nombre = "Electronica",
            Descripcion = "Vieja descripcion"
        };

        var updateDto = new UpdateCategoriaDto
        {
            Nombre = "Electronica Actualizada",
            Descripcion = "Nueva descripcion",
            CategoriaPadre = null
        };

        _categoriaRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(existingCategoria);

        _categoriaRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Categorium>()))
            .Returns(Task.CompletedTask);

        var result = await _categoriaService.UpdateAsync(1, updateDto);

        result.Should().NotBeNull();
        result.Nombre.Should().Be(updateDto.Nombre);
        result.Descripcion.Should().Be(updateDto.Descripcion);
    }

    [Fact]
    public async Task UpdateAsync_CategoriaNoExiste_RetornaNull()
    {
        var updateDto = new UpdateCategoriaDto
        {
            Nombre = "Categoria Test",
            Descripcion = "Descripcion Test"
        };

        _categoriaRepositoryMock
            .Setup(x => x.GetByIdAsync(999))
            .ReturnsAsync((Categorium?)null);

        var result = await _categoriaService.UpdateAsync(999, updateDto);

        result.Should().BeNull();
    }

    #endregion

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_CategoriaExistente_RetornaTrue()
    {
        var categoria = new Categorium
        {
            Id = 1,
            Nombre = "Categoria Test"
        };

        _categoriaRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(categoria);

        _categoriaRepositoryMock
            .Setup(x => x.DeleteAsync(It.IsAny<Categorium>()))
            .Returns(Task.CompletedTask);

        var result = await _categoriaService.DeleteAsync(1);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_CategoriaNoExiste_RetornaFalse()
    {
        _categoriaRepositoryMock
            .Setup(x => x.GetByIdAsync(999))
            .ReturnsAsync((Categorium?)null);

        var result = await _categoriaService.DeleteAsync(999);

        result.Should().BeFalse();
    }

    #endregion
}
