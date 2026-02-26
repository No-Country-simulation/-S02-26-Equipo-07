using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.DTOs.Documentos;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentosController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly ILogger<DocumentosController> _logger;

        public DocumentosController(IDocumentService documentService, ILogger<DocumentosController> logger)
        {
            _documentService = documentService;
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

        // ============ FACTURAS ============

        /// <summary>
        /// Emite una nueva factura.
        /// </summary>
        [HttpPost("facturas")]
        public async Task<ActionResult<GetFacturaDto>> EmitirFactura([FromBody] CreateFacturaDto dto)
        {
            try
            {
                if (dto.VentaId <= 0)
                    return BadRequest(new { message = "VentaId es requerido" });

                var userId = GetUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "No se pudo obtener el ID del usuario" });

                var numeroFactura = await _documentService.GenerarNumeroDocumentoAsync("FACTURA");
                if (string.IsNullOrEmpty(numeroFactura))
                    return BadRequest(new { message = "Error al generar número de factura" });

                var factura = new Factura
                {
                    VentaId = dto.VentaId,
                    NumeroFactura = numeroFactura,
                    TipoFactura = dto.TipoFactura,
                    CUIT = dto.CUIT,
                    CUITCliente = dto.CUITCliente,
                    TipoDocumentoCliente = dto.TipoDocumentoCliente,
                    NumeroDocumentoCliente = dto.NumeroDocumentoCliente,
                    NombreCliente = dto.NombreCliente,
                    CondicionIva = dto.CondicionIva,
                    Subtotal = dto.Subtotal,
                    Descuento = dto.Descuento,
                    IVA = dto.IVA,
                    PorcentajeIVA = dto.PorcentajeIVA,
                    IIBB = dto.IIBB,
                    Total = dto.Total,
                    Observaciones = dto.Observaciones
                };

                var facturaId = await _documentService.EmitirFacturaAsync(factura, userId);

                if (facturaId == -1)
                    return BadRequest(new { message = "No se pudo emitir la factura" });

                var facturaCreada = await _documentService.ObtenerFacturaAsync(facturaId);
                var facturaDto = MapFacturaToDto(facturaCreada!);

                return CreatedAtAction(nameof(ObtenerFactura), new { id = facturaId }, facturaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error emitting factura");
                return StatusCode(500, new { message = "An error occurred while emitting factura" });
            }
        }

        /// <summary>
        /// Obtiene una factura específica.
        /// </summary>
        [HttpGet("facturas/{id:long}")]
        public async Task<ActionResult<GetFacturaDto>> ObtenerFactura(long id)
        {
            try
            {
                var factura = await _documentService.ObtenerFacturaAsync(id);
                if (factura == null)
                    return NotFound(new { message = "Factura no encontrada" });

                var facturaDto = MapFacturaToDto(factura);
                return Ok(facturaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving factura");
                return StatusCode(500, new { message = "An error occurred while retrieving factura" });
            }
        }

        /// <summary>
        /// Obtiene una factura por su número.
        /// </summary>
        [HttpGet("facturas/numero/{numeroFactura}")]
        public async Task<ActionResult<GetFacturaDto>> ObtenerFacturaPorNumero(string numeroFactura)
        {
            try
            {
                var factura = await _documentService.ObtenerFacturaPorNumeroAsync(numeroFactura);
                if (factura == null)
                    return NotFound(new { message = "Factura no encontrada" });

                var facturaDto = MapFacturaToDto(factura);
                return Ok(facturaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving factura");
                return StatusCode(500, new { message = "An error occurred while retrieving factura" });
            }
        }

        /// <summary>
        /// Lista todas las facturas de una venta.
        /// </summary>
        [HttpGet("facturas/venta/{ventaId:long}")]
        public async Task<ActionResult<IEnumerable<GetFacturaDto>>> ObtenerFacturasPorVenta(long ventaId)
        {
            try
            {
                var facturas = await _documentService.ObtenerFacturasPorVentaAsync(ventaId);
                var facturasDto = facturas.Select(MapFacturaToDto);

                return Ok(facturasDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving facturas");
                return StatusCode(500, new { message = "An error occurred while retrieving facturas" });
            }
        }

        /// <summary>
        /// Anula una factura.
        /// </summary>
        [HttpDelete("facturas/{id:long}")]
        public async Task<ActionResult> AnularFactura(long id)
        {
            try
            {
                var userId = GetUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "No se pudo obtener el ID del usuario" });

                var exito = await _documentService.AnularFacturaAsync(id, userId);
                if (!exito)
                    return BadRequest(new { message = "No se pudo anular la factura" });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error canceling factura");
                return StatusCode(500, new { message = "An error occurred while canceling factura" });
            }
        }

        // ============ NOTAS DE CRÉDITO ============

        /// <summary>
        /// Emite una nueva nota de crédito.
        /// </summary>
        [HttpPost("notas-credito")]
        public async Task<ActionResult<GetNotaCreditoDto>> EmitirNotaCredito([FromBody] CreateNotaCreditoDto dto)
        {
            try
            {
                if (dto.FacturaId <= 0)
                    return BadRequest(new { message = "FacturaId es requerido" });

                var userId = GetUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "No se pudo obtener el ID del usuario" });

                var numeroNC = await _documentService.GenerarNumeroDocumentoAsync("NC");
                if (string.IsNullOrEmpty(numeroNC))
                    return BadRequest(new { message = "Error al generar número de nota de crédito" });

                var notaCredito = new NotaCredito
                {
                    FacturaId = dto.FacturaId,
                    NumeroNotaCredito = numeroNC,
                    TipoNotaCredito = dto.TipoNotaCredito,
                    Razon = dto.Razon,
                    Descripcion = dto.Descripcion,
                    CUITCliente = dto.CUITCliente,
                    NombreCliente = dto.NombreCliente,
                    Monto = dto.Monto,
                    IVA = dto.IVA,
                    PorcentajeIVA = dto.PorcentajeIVA,
                    IIBB = dto.IIBB,
                    Total = dto.Total
                };

                var notaCreditoId = await _documentService.EmitirNotaCreditoAsync(notaCredito, userId);

                if (notaCreditoId == -1)
                    return BadRequest(new { message = "No se pudo emitir la nota de crédito" });

                var notaCreditoCreada = await _documentService.ObtenerNotaCreditoAsync(notaCreditoId);
                var notaCreditoDto = MapNotaCreditoToDto(notaCreditoCreada!);

                return CreatedAtAction(nameof(ObtenerNotaCredito), new { id = notaCreditoId }, notaCreditoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error emitting nota de crédito");
                return StatusCode(500, new { message = "An error occurred while emitting nota de crédito" });
            }
        }

        /// <summary>
        /// Obtiene una nota de crédito.
        /// </summary>
        [HttpGet("notas-credito/{id:long}")]
        public async Task<ActionResult<GetNotaCreditoDto>> ObtenerNotaCredito(long id)
        {
            try
            {
                var notaCredito = await _documentService.ObtenerNotaCreditoAsync(id);
                if (notaCredito == null)
                    return NotFound(new { message = "Nota de crédito no encontrada" });

                var notaCreditoDto = MapNotaCreditoToDto(notaCredito);
                return Ok(notaCreditoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving nota de crédito");
                return StatusCode(500, new { message = "An error occurred while retrieving nota de crédito" });
            }
        }

        /// <summary>
        /// Lista todas las notas de crédito de una factura.
        /// </summary>
        [HttpGet("notas-credito/factura/{facturaId:long}")]
        public async Task<ActionResult<IEnumerable<GetNotaCreditoDto>>> ObtenerNotasCreditoPorFactura(long facturaId)
        {
            try
            {
                var notas = await _documentService.ObtenerNotasCreditoPorFacturaAsync(facturaId);
                var notasDto = notas.Select(MapNotaCreditoToDto);

                return Ok(notasDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving notas de crédito");
                return StatusCode(500, new { message = "An error occurred while retrieving notas de crédito" });
            }
        }

        /// <summary>
        /// Anula una nota de crédito.
        /// </summary>
        [HttpDelete("notas-credito/{id:long}")]
        public async Task<ActionResult> AnularNotaCredito(long id)
        {
            try
            {
                var userId = GetUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "No se pudo obtener el ID del usuario" });

                var exito = await _documentService.AnularNotaCreditoAsync(id, userId);
                if (!exito)
                    return BadRequest(new { message = "No se pudo anular la nota de crédito" });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error canceling nota de crédito");
                return StatusCode(500, new { message = "An error occurred while canceling nota de crédito" });
            }
        }

        // ============ NOTAS DE DÉBITO ============

        /// <summary>
        /// Emite una nueva nota de débito.
        /// </summary>
        [HttpPost("notas-debito")]
        public async Task<ActionResult<GetNotaDebitoDto>> EmitirNotaDebito([FromBody] CreateNotaDebitoDto dto)
        {
            try
            {
                if (dto.FacturaId <= 0)
                    return BadRequest(new { message = "FacturaId es requerido" });

                var userId = GetUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "No se pudo obtener el ID del usuario" });

                var numeroND = await _documentService.GenerarNumeroDocumentoAsync("ND");
                if (string.IsNullOrEmpty(numeroND))
                    return BadRequest(new { message = "Error al generar número de nota de débito" });

                var notaDebito = new NotaDebito
                {
                    FacturaId = dto.FacturaId,
                    NumeroNotaDebito = numeroND,
                    TipoNotaDebito = dto.TipoNotaDebito,
                    Razon = dto.Razon,
                    Descripcion = dto.Descripcion,
                    CUITCliente = dto.CUITCliente,
                    NombreCliente = dto.NombreCliente,
                    Monto = dto.Monto,
                    IVA = dto.IVA,
                    PorcentajeIVA = dto.PorcentajeIVA,
                    IIBB = dto.IIBB,
                    Total = dto.Total
                };

                var notaDebitoId = await _documentService.EmitirNotaDebitoAsync(notaDebito, userId);

                if (notaDebitoId == -1)
                    return BadRequest(new { message = "No se pudo emitir la nota de débito" });

                var notaDebitoCreada = await _documentService.ObtenerNotaDebitoAsync(notaDebitoId);
                var notaDebitoDto = MapNotaDebitoToDto(notaDebitoCreada!);

                return CreatedAtAction(nameof(ObtenerNotaDebito), new { id = notaDebitoId }, notaDebitoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error emitting nota de débito");
                return StatusCode(500, new { message = "An error occurred while emitting nota de débito" });
            }
        }

        /// <summary>
        /// Obtiene una nota de débito.
        /// </summary>
        [HttpGet("notas-debito/{id:long}")]
        public async Task<ActionResult<GetNotaDebitoDto>> ObtenerNotaDebito(long id)
        {
            try
            {
                var notaDebito = await _documentService.ObtenerNotaDebitoAsync(id);
                if (notaDebito == null)
                    return NotFound(new { message = "Nota de débito no encontrada" });

                var notaDebitoDto = MapNotaDebitoToDto(notaDebito);
                return Ok(notaDebitoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving nota de débito");
                return StatusCode(500, new { message = "An error occurred while retrieving nota de débito" });
            }
        }

        /// <summary>
        /// Lista todas las notas de débito de una factura.
        /// </summary>
        [HttpGet("notas-debito/factura/{facturaId:long}")]
        public async Task<ActionResult<IEnumerable<GetNotaDebitoDto>>> ObtenerNotasDebitoPorFactura(long facturaId)
        {
            try
            {
                var notas = await _documentService.ObtenerNotasDebitoPorFacturaAsync(facturaId);
                var notasDto = notas.Select(MapNotaDebitoToDto);

                return Ok(notasDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving notas de débito");
                return StatusCode(500, new { message = "An error occurred while retrieving notas de débito" });
            }
        }

        /// <summary>
        /// Anula una nota de débito.
        /// </summary>
        [HttpDelete("notas-debito/{id:long}")]
        public async Task<ActionResult> AnularNotaDebito(long id)
        {
            try
            {
                var userId = GetUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "No se pudo obtener el ID del usuario" });

                var exito = await _documentService.AnularNotaDebitoAsync(id, userId);
                if (!exito)
                    return BadRequest(new { message = "No se pudo anular la nota de débito" });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error canceling nota de débito");
                return StatusCode(500, new { message = "An error occurred while canceling nota de débito" });
            }
        }

        // ============ REPORTES ============

        /// <summary>
        /// Obtiene resumen de documentos en un rango de fechas.
        /// </summary>
        [HttpGet("reportes/resumen")]
        public async Task<ActionResult<ResumenDocumentosDto>> ObtenerResumenDocumentos(
            [FromQuery] DateTime desde,
            [FromQuery] DateTime hasta)
        {
            try
            {
                if (desde > hasta)
                    return BadRequest(new { message = "La fecha 'desde' no puede ser mayor a 'hasta'" });

                var (totalFacturas, montoFacturas, totalNotas, montoNotas) = 
                    await _documentService.ObtenerResumenDocumentosAsync(desde, hasta);

                var resumenDto = new ResumenDocumentosDto
                {
                    TotalFacturas = totalFacturas,
                    MontoFacturas = montoFacturas,
                    TotalNotas = totalNotas,
                    MontoNotas = montoNotas
                };

                return Ok(resumenDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving resumen documentos");
                return StatusCode(500, new { message = "An error occurred while retrieving resumen documentos" });
            }
        }

        // ============ MAPPERS ============

        private GetFacturaDto MapFacturaToDto(Factura factura)
        {
            return new GetFacturaDto
            {
                Id = factura.Id,
                VentaId = factura.VentaId,
                NumeroFactura = factura.NumeroFactura,
                TipoFactura = factura.TipoFactura,
                FechaEmision = factura.FechaEmision,
                FechaVencimiento = factura.FechaVencimiento,
                Estado = factura.Estado,
                NombreCliente = factura.NombreCliente,
                Total = factura.Total,
                CAE = factura.CAE,
                FechaVencimientoCae = factura.FechaVencimientoCae
            };
        }

        private GetNotaCreditoDto MapNotaCreditoToDto(NotaCredito notaCredito)
        {
            return new GetNotaCreditoDto
            {
                Id = notaCredito.Id,
                FacturaId = notaCredito.FacturaId,
                NumeroNotaCredito = notaCredito.NumeroNotaCredito,
                FechaEmision = notaCredito.FechaEmision,
                Razon = notaCredito.Razon,
                Estado = notaCredito.Estado,
                NombreCliente = notaCredito.NombreCliente,
                Total = notaCredito.Total
            };
        }

        private GetNotaDebitoDto MapNotaDebitoToDto(NotaDebito notaDebito)
        {
            return new GetNotaDebitoDto
            {
                Id = notaDebito.Id,
                FacturaId = notaDebito.FacturaId,
                NumeroNotaDebito = notaDebito.NumeroNotaDebito,
                FechaEmision = notaDebito.FechaEmision,
                Razon = notaDebito.Razon,
                Estado = notaDebito.Estado,
                NombreCliente = notaDebito.NombreCliente,
                Total = notaDebito.Total
            };
        }
    }
}
