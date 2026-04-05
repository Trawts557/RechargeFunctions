using Microsoft.AspNetCore.Mvc;
using RechargeFunctions.Api.Request.Recarga;
using RechargeFunctions.Application.Services;
using RechargeFunctions.Domain.Enums.Recarga;


namespace RechargeFunctions.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecargasController : ControllerBase
    {
        private readonly RecargaService _recargaService;

        public RecargasController(RecargaService recargaService)
        {
            _recargaService = recargaService;
        }

        [HttpPost]
        public async Task<ActionResult> CrearRecarga([FromBody] CreateRecargaRequest request)
        {
            var result = await _recargaService.CrearRecargaAsync(
                request.ClienteId, 
                request.TarjetaId, 
                request.MontoRecarga, 
                request.EstaPagada);

            return result switch
            {
                CrearRecargaResult.Success => Ok("Recarga creada correctamente"),
                CrearRecargaResult.InvalidAmount => BadRequest("No se puede realizar una recarga con monto igual o menor a 0"),
                CrearRecargaResult.ClientNotFound => NotFound("Cliente no encontrado"),
                CrearRecargaResult.CardNotFound => NotFound("Tarjeta no encontrada"),
                CrearRecargaResult.CardInactive => BadRequest("La tarjeta no esta activa"),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Ocurrio un error inesperado")

            };
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> EditarRecarga(int id, [FromBody] UpdateRecargaRequest request)
        {
            var result = await _recargaService.EditarRecargaAsync(
                id,
                request.ClienteId,
                request.TarjetaId,
                request.Monto,
                request.EstaPagada);

            return result switch 
            { 
                EditarRecargaResult.Success => NoContent(),
                EditarRecargaResult.CardInactive => Conflict("La tarjeta esta inactiva"),
                EditarRecargaResult.RechargeNotFound => NotFound("No se encontro la recarga"),
                EditarRecargaResult.CardNotFound => NotFound("No se encontro la tarjeta"),
                EditarRecargaResult.ClientNotFound => NotFound("No se encontro el cliente"),
                EditarRecargaResult.InvalidCardId => BadRequest("Id de tarjeta invalida"),
                EditarRecargaResult.InvalidClientId => BadRequest ("Id de cliente invalida"),
                EditarRecargaResult.InvalidAmount => BadRequest("El monto es invalido"),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Ocurrio un error inesperado")
            };

        }

        [HttpPatch("{id:int}/pagar")]
        public async Task<ActionResult> MarcarComoPagada(int id)
        {
            var result = await _recargaService.MarcarRecargaComoPagadaAsync(id);

            return result switch
            {
                PagarRecargaResult.Success => NoContent(),
                PagarRecargaResult.RechargeNotFound => NotFound("No se encontro la recarga"),
                PagarRecargaResult.RechargeAlreadyPaid => Conflict("Esa recarga ya esta pagada"),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Ocurrio un error inesperado")
            };
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> EliminarRecarga(int id)
        {
            var result = await _recargaService.EliminarRecargaAsync(id);
            
            return result switch
            {
                EliminarRecargaResult.Success => NoContent(),
                EliminarRecargaResult.RechargeNotFound => NotFound("No se encontro la recarga"),
                EliminarRecargaResult.IsAlreadyDeleted => Conflict("Esa recarga ya esta eliminada"),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Ocurrio un error inesperado")
            };
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerRecargas()
        {
            var recargas = await _recargaService.ObtenerRecargasAsync();

            return Ok(recargas);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult> ObtenerRecargaPorId(int id)
        {
            var recarga = await _recargaService.ObtenerRecargaPorIdAsync(id);

            if (recarga == null)
            {
                return NotFound("No se encontro la recarga");
            }

            return Ok(recarga);
        }

    }
}
