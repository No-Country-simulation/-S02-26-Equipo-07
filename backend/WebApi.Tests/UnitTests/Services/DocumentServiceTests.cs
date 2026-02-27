using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Models;
using WebApi.Services;
using Xunit;

namespace WebApi.Tests.UnitTests.Services;

public class DocumentServiceTests
{
    private readonly Mock<ILogger<DocumentService>> _loggerMock;

    public DocumentServiceTests()
    {
        _loggerMock = new Mock<ILogger<DocumentService>>();
    }

    [Fact]
    public void DocumentService_Constructor_InitializesCorrectly()
    {
        var contextMock = new Mock<WebApi.Data.NC07WebAppContext>();
        
        var service = new DocumentService(contextMock.Object, _loggerMock.Object);

        service.Should().NotBeNull();
    }

    [Fact]
    public void EmitirFacturaAsync_SinVentaId_RetornaMenosUno()
    {
        var contextMock = new Mock<WebApi.Data.NC07WebAppContext>();
        var service = new DocumentService(contextMock.Object, _loggerMock.Object);

        var factura = new Factura
        {
            VentaId = 0,
            NumeroFactura = "FAC-001"
        };

        var result = service.EmitirFacturaAsync(factura, 1);

        result.Result.Should().Be(-1);
    }

    [Fact]
    public void EmitirNotaCreditoAsync_SinFacturaId_RetornaMenosUno()
    {
        var contextMock = new Mock<WebApi.Data.NC07WebAppContext>();
        var service = new DocumentService(contextMock.Object, _loggerMock.Object);

        var notaCredito = new NotaCredito
        {
            FacturaId = 0,
            NumeroNotaCredito = "NC-001"
        };

        var result = service.EmitirNotaCreditoAsync(notaCredito, 1);

        result.Result.Should().Be(-1);
    }

    [Fact]
    public void EmitirNotaDebitoAsync_SinFacturaId_RetornaMenosUno()
    {
        var contextMock = new Mock<WebApi.Data.NC07WebAppContext>();
        var service = new DocumentService(contextMock.Object, _loggerMock.Object);

        var notaDebito = new NotaDebito
        {
            FacturaId = 0,
            NumeroNotaDebito = "ND-001"
        };

        var result = service.EmitirNotaDebitoAsync(notaDebito, 1);

        result.Result.Should().Be(-1);
    }
}
