using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Services;
using Xunit;

namespace WebApi.Tests.UnitTests.Services;

public class VentaServiceTests
{
    private readonly Mock<ILogger<VentaService>> _loggerMock;
    private readonly Mock<IStockService> _stockServiceMock;

    public VentaServiceTests()
    {
        _loggerMock = new Mock<ILogger<VentaService>>();
        _stockServiceMock = new Mock<IStockService>();
    }

    [Fact]
    public void VentaService_Constructor_InitializesCorrectly()
    {
        var contextMock = new Mock<WebApi.Data.NC07WebAppContext>();
        
        var service = new VentaService(contextMock.Object, _stockServiceMock.Object, _loggerMock.Object);

        service.Should().NotBeNull();
    }
}
