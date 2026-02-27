using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Models;
using WebApi.Services;
using Xunit;

namespace WebApi.Tests.UnitTests.Services;

public class PaymentServiceTests
{
    private readonly Mock<ILogger<PaymentService>> _loggerMock;

    public PaymentServiceTests()
    {
        _loggerMock = new Mock<ILogger<PaymentService>>();
    }

    [Fact]
    public void PaymentService_Constructor_InitializesCorrectly()
    {
        var contextMock = new Mock<WebApi.Data.NC07WebAppContext>();
        
        var service = new PaymentService(contextMock.Object, _loggerMock.Object);

        service.Should().NotBeNull();
    }

    [Fact]
    public void RegistrarPagoAsync_ConMontoCero_RetornaMenosUno()
    {
        var contextMock = new Mock<WebApi.Data.NC07WebAppContext>();
        var service = new PaymentService(contextMock.Object, _loggerMock.Object);

        var pago = new Pago
        {
            VentaId = 1,
            Monto = 0,
            MetodoPago = "EFECTIVO"
        };

        var result = service.RegistrarPagoAsync(pago, 1);

        result.Result.Should().Be(-1);
    }

    [Fact]
    public void RegistrarPagoAsync_ConMontoNegativo_RetornaMenosUno()
    {
        var contextMock = new Mock<WebApi.Data.NC07WebAppContext>();
        var service = new PaymentService(contextMock.Object, _loggerMock.Object);

        var pago = new Pago
        {
            VentaId = 1,
            Monto = -100,
            MetodoPago = "EFECTIVO"
        };

        var result = service.RegistrarPagoAsync(pago, 1);

        result.Result.Should().Be(-1);
    }
}
