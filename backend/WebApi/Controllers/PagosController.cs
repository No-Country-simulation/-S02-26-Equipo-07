using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.DTOs.Pagos;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PagosController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PagosController> _logger;

        public PagosController(IPaymentService paymentService, ILogger<PagosController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene el ID del usuario autenticado desde el token JWT.
        /// </summary>
        private long GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null && long.TryParse(userIdClaim.Value, out var userId) ? userId : 0;
        }

        /// <summary>
        /// Registra un nuevo pago para una venta.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<GetPagoDto>> RegistrarPago([FromBody] CreatePagoDto dto)
        {
            try
            {
                if (dto.VentaId <= 0 || dto.Monto <= 0)
                    return BadRequest(new { message = "VentaId y Monto son requeridos y deben ser mayores a 0" });

                var userId = GetUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "No se pudo obtener el ID del usuario" });

                var pago = new Pago
                {
                    VentaId = dto.VentaId,
                    Monto = dto.Monto,
                    MetodoPago = dto.MetodoPago,
                    ReferenciaPago = dto.ReferenciaPago,
                    Observaciones = dto.Observaciones
                };

                var pagoId = await _paymentService.RegistrarPagoAsync(pago, userId);

                if (pagoId == -1)
                    return BadRequest(new { message = "No se pudo registrar el pago. Verifique que la venta existe y el monto no exceda el total" });

                var pagoCreado = await _paymentService.ObtenerPagoAsync(pagoId);
                var pagoDto = MapPagoToDto(pagoCreado!);

                return CreatedAtAction(nameof(ObtenerPago), new { id = pagoId }, pagoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering pago");
                return StatusCode(500, new { message = "An error occurred while registering pago" });
            }
        }

        /// <summary>
        /// Obtiene un pago específico.
        /// </summary>
        [HttpGet("{id:long}")]
        public async Task<ActionResult<GetPagoDto>> ObtenerPago(long id)
        {
            try
            {
                var pago = await _paymentService.ObtenerPagoAsync(id);
                if (pago == null)
                    return NotFound(new { message = "Pago no encontrado" });

                var pagoDto = MapPagoToDto(pago);
                return Ok(pagoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pago");
                return StatusCode(500, new { message = "An error occurred while retrieving pago" });
            }
        }

        /// <summary>
        /// Lista todos los pagos de una venta.
        /// </summary>
        [HttpGet("venta/{ventaId:long}")]
        public async Task<ActionResult<IEnumerable<GetPagoDto>>> ObtenerPagosPorVenta(long ventaId)
        {
            try
            {
                var pagos = await _paymentService.ObtenerPagosPorVentaAsync(ventaId);
                var pagosDto = pagos.Select(MapPagoToDto);

                return Ok(pagosDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pagos");
                return StatusCode(500, new { message = "An error occurred while retrieving pagos" });
            }
        }

        /// <summary>
        /// Obtiene el estado de pago de una venta (monto pagado vs total).
        /// </summary>
        [HttpGet("venta/{ventaId:long}/estado")]
        public async Task<ActionResult<EstadoPagoDto>> ObtenerEstadoPago(long ventaId)
        {
            try
            {
                var (montoPagado, total, saldo) = await _paymentService.ObtenerEstadoPagoVentaAsync(ventaId);

                var estadoDto = new EstadoPagoDto
                {
                    VentaId = ventaId,
                    MontoPagado = montoPagado,
                    Total = total,
                    Saldo = saldo
                };

                return Ok(estadoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving estado pago");
                return StatusCode(500, new { message = "An error occurred while retrieving estado pago" });
            }
        }

        /// <summary>
        /// Confirma un pago registrado.
        /// </summary>
        [HttpPatch("{id:long}/confirmar")]
        public async Task<ActionResult> ConfirmarPago(long id)
        {
            try
            {
                var userId = GetUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "No se pudo obtener el ID del usuario" });

                var exito = await _paymentService.ConfirmarPagoAsync(id, userId);
                if (!exito)
                    return BadRequest(new { message = "No se pudo confirmar el pago" });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming pago");
                return StatusCode(500, new { message = "An error occurred while confirming pago" });
            }
        }

        /// <summary>
        /// Anula un pago registrado.
        /// </summary>
        [HttpDelete("{id:long}")]
        public async Task<ActionResult> AnularPago(long id)
        {
            try
            {
                var userId = GetUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "No se pudo obtener el ID del usuario" });

                var exito = await _paymentService.AnularPagoAsync(id, userId);
                if (!exito)
                    return BadRequest(new { message = "No se pudo anular el pago" });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error canceling pago");
                return StatusCode(500, new { message = "An error occurred while canceling pago" });
            }
        }

        /// <summary>
        /// Obtiene resumen de pagos por método en un rango de fechas.
        /// </summary>
        [HttpGet("reportes/resumen")]
        public async Task<ActionResult<IEnumerable<ResumenPagosDto>>> ObtenerResumenPagos(
            [FromQuery] DateTime desde,
            [FromQuery] DateTime hasta)
        {
            try
            {
                if (desde > hasta)
                    return BadRequest(new { message = "La fecha 'desde' no puede ser mayor a 'hasta'" });

                var resumen = await _paymentService.ObtenerResumenPagosAsync(desde, hasta);
                var resumenDto = resumen.Select(r => new ResumenPagosDto
                {
                    MetodoPago = r.MetodoPago,
                    Total = r.Total,
                    Cantidad = r.Cantidad
                });

                return Ok(resumenDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving resumen pagos");
                return StatusCode(500, new { message = "An error occurred while retrieving resumen pagos" });
            }
        }

        /// <summary>
        /// Mapea una entidad Pago a su DTO.
        /// </summary>
        private GetPagoDto MapPagoToDto(Pago pago)
        {
            return new GetPagoDto
            {
                Id = pago.Id,
                VentaId = pago.VentaId,
                Monto = pago.Monto,
                MetodoPago = pago.MetodoPago,
                ReferenciaPago = pago.ReferenciaPago,
                Estado = pago.Estado,
                FechaPago = pago.FechaPago,
                FechaConfirmacion = pago.FechaConfirmacion,
                Observaciones = pago.Observaciones
            };
        }
    }
}
