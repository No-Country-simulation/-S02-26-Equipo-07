using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.DTOs.Ventas;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly IVentaService _ventaService;
        private readonly ILogger<VentasController> _logger;

        public VentasController(IVentaService ventaService, ILogger<VentasController> logger)
        {
            _ventaService = ventaService;
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
        /// Crea una nueva venta con sus detalles.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<GetVentaDto>> CrearVenta([FromBody] CreateVentaDto dto)
        {
            try
            {
                if (dto == null || dto.Detalles.Count == 0)
                    return BadRequest(new { message = "La venta debe incluir al menos un detalle" });

                var userId = GetUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "No se pudo obtener el ID del usuario" });

                var venta = new Venta
                {
                    FechaVenta = dto.FechaVenta,
                    SubTotal = dto.SubTotal,
                    Descuento = dto.Descuento,
                    IVA = dto.IVA,
                    Total = dto.Total,
                    Observaciones = dto.Observaciones
                };

                var detalles = dto.Detalles.Select(d => new DetalleVenta
                {
                    ProductoId = d.ProductoId,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Descuento = d.Descuento,
                    Subtotal = d.Subtotal
                }).ToList();

                var ventaId = await _ventaService.CrearVentaAsync(venta, detalles, userId);

                if (ventaId == -1)
                    return BadRequest(new { message = "No hay suficiente stock para completar la venta" });

                var ventaCreada = await _ventaService.ObtenerVentaAsync(ventaId);
                var ventaDto = MapVentaToDto(ventaCreada!);

                return CreatedAtAction(nameof(ObtenerVenta), new { id = ventaId }, ventaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating venta");
                return StatusCode(500, new { message = "An error occurred while creating venta" });
            }
        }

        /// <summary>
        /// Obtiene una venta específica.
        /// </summary>
        [HttpGet("{id:long}")]
        public async Task<ActionResult<GetVentaDto>> ObtenerVenta(long id)
        {
            try
            {
                var venta = await _ventaService.ObtenerVentaAsync(id);
                if (venta == null)
                    return NotFound(new { message = "Venta no encontrada" });

                var ventaDto = MapVentaToDto(venta);
                return Ok(ventaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving venta");
                return StatusCode(500, new { message = "An error occurred while retrieving venta" });
            }
        }

        /// <summary>
        /// Lista todas las ventas del usuario autenticado.
        /// </summary>
        [HttpGet("usuario")]
        public async Task<ActionResult<IEnumerable<GetVentaDto>>> ListarVentasUsuario()
        {
            try
            {
                var userId = GetUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "No se pudo obtener el ID del usuario" });

                var ventas = await _ventaService.ListarVentasUsuarioAsync(userId);
                var ventasDto = ventas.Select(MapVentaToDto);

                return Ok(ventasDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ventas");
                return StatusCode(500, new { message = "An error occurred while retrieving ventas" });
            }
        }

        /// <summary>
        /// Anula una venta y revierte el stock.
        /// </summary>
        [HttpDelete("{id:long}")]
        public async Task<ActionResult> AnularVenta(long id)
        {
            try
            {
                var userId = GetUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "No se pudo obtener el ID del usuario" });

                var exito = await _ventaService.AnularVentaAsync(id, userId);
                if (!exito)
                    return BadRequest(new { message = "No se pudo anular la venta" });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error canceling venta");
                return StatusCode(500, new { message = "An error occurred while canceling venta" });
            }
        }

        /// <summary>
        /// Mapea una entidad Venta a su DTO.
        /// </summary>
        private GetVentaDto MapVentaToDto(Venta venta)
        {
            return new GetVentaDto
            {
                Id = venta.Id,
                UserId = venta.UserId,
                FechaVenta = venta.FechaVenta,
                SubTotal = venta.SubTotal,
                Descuento = venta.Descuento,
                IVA = venta.IVA,
                Total = venta.Total,
                Estado = venta.Estado,
                Observaciones = venta.Observaciones,
                FechaCreacion = venta.FechaCreacion,
                FechaActualizacion = venta.FechaActualizacion,
                Detalles = venta.DetallesVenta.Select(d => new GetDetalleVentaDto
                {
                    Id = d.Id,
                    ProductoId = d.ProductoId,
                    NombreProducto = d.Producto?.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Descuento = d.Descuento,
                    Subtotal = d.Subtotal
                }).ToList()
            };
        }
    }
}
