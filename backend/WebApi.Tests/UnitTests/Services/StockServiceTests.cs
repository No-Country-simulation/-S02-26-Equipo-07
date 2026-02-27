using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Services;
using Xunit;

namespace WebApi.Tests.UnitTests.Services;

public class StockServiceTests
{
    private readonly Mock<ILogger<StockService>> _loggerMock;

    public StockServiceTests()
    {
        _loggerMock = new Mock<ILogger<StockService>>();
    }

    [Fact]
    public void StockService_Constructor_InitializesCorrectly()
    {
        var contextMock = new Mock<WebApi.Data.NC07WebAppContext>();
        
        var service = new StockService(contextMock.Object, _loggerMock.Object);

        service.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void IngresarProductoAsync_ConCantidadInvalida_RetornaFalse(long cantidad)
    {
        var contextMock = new Mock<WebApi.Data.NC07WebAppContext>();
        var service = new StockService(contextMock.Object, _loggerMock.Object);

        var result = service.IngresarProductoAsync(1, cantidad, "Test", 1);

        result.Result.Should().BeFalse();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void EgresarProductoAsync_ConCantidadInvalida_RetornaFalse(long cantidad)
    {
        var contextMock = new Mock<WebApi.Data.NC07WebAppContext>();
        var service = new StockService(contextMock.Object, _loggerMock.Object);

        var result = service.EgresarProductoAsync(1, cantidad, "Test", 1);

        result.Result.Should().BeFalse();
    }
}
