using Microsoft.AspNetCore.Mvc;
using RechargeFunctions.Api.Request.Cliente;
using RechargeFunctions.Application.Services;
using RechargeFunctions.Domain.Enums.Tarjeta;

namespace RechargeFunctions.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarjetasController : ControllerBase
    {
        private readonly TarjetaService _tarjetaService;
        
        public TarjetasController(TarjetaService tarjetaService)
        {
            _tarjetaService = tarjetaService;
        }

        [HttpPost]
        public async Task<ActionResult> AgregarTarjeta([FromBody] CreateTarjetaRequest request)
        {
            var result = await _tarjetaService.AgregarTarjetaAsync(
                request.Nombre, 
                request.UltimosDigitos);

            return result switch
            {
                CrearTarjetaResult.Success => Ok("Tarjeta creada correctamente"),
                CrearTarjetaResult.InvalidData => BadRequest("Datos invalidos"),
                CrearTarjetaResult.CardAlreadyExists => Conflict("Ya existe esa tarjeta"),
                CrearTarjetaResult.InvalidLastDigits => BadRequest("La tarjeta debe tener mas de 3 digitos"),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Ocurrio un error inesperado")
            };

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> EditarTarjeta(int id, [FromBody] UpdateTarjetaRequest request)
        {
            var result = await _tarjetaService.EditarTarjetaAsync(
                id, 
                request.Nombre,
                request.UltimosDigitos);

            return result switch
            {
                EditarTarjetaResult.Success => NoContent(),
                EditarTarjetaResult.InvalidData => BadRequest("Datos invalidos"),
                EditarTarjetaResult.CardNotFound => NotFound("No se pudo encontrar la tarjeta"),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Ocurrio un error inesperado")
            };
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DesactivarTarjeta(int id)
        {
            var result = await _tarjetaService.DesactivarTarjetaAsync(id);

            return result switch
            {
                EliminarTarjetaResult.Success => NoContent(),
                EliminarTarjetaResult.CardNotFound => NotFound("Tarjeta no encontrada"),
                EliminarTarjetaResult.IsAlreadyDeleted => Conflict("La tarjeta ya esta desactivada"),
                EliminarTarjetaResult.InvalidData => BadRequest("Datos invalidos"),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Ocurrio un error inesperado")
            };
        }

        [HttpPatch("{id:int}/activar")]
        public async Task<ActionResult> ActivarTarjeta(int id)
        {
            var result = await _tarjetaService.ActivarTarjetaAsync(id);

            return result switch
            {
                ActivarTarjetaResult.Success => NoContent(),
                ActivarTarjetaResult.CardNotFound => NotFound("Tarjeta no encontrada"),
                ActivarTarjetaResult.CardIsAlreadyActive => Conflict("La tarjeta ya está activa"),
                ActivarTarjetaResult.InvalidData => BadRequest("Datos inválidos"),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error inesperado")
            };
        }


        [HttpGet]
        public async Task<ActionResult> ObtenerTarjetas()
        {
            var tarjetas = await _tarjetaService.ObtenerTarjetasAsync();

            return Ok(tarjetas);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> ObtenerTarjetaPorId(int id)
        {
            var tarjeta = await _tarjetaService.ObtenerTarjetaPorIdAsync(id);

            if (tarjeta == null)
            {
                return NotFound("No se encontro la tarjeta");
            }

            return Ok(tarjeta);
        }

        

    }
}
