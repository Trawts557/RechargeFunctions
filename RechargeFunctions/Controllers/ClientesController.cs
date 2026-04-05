using Microsoft.AspNetCore.Mvc;
using RechargeFunctions.Api.Request.Cliente;
using RechargeFunctions.Application.Services;
using RechargeFunctions.Domain.Enums.Cliente;


namespace RechargeFunctions.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClientesController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost]
        public async Task<ActionResult> AgregarCliente([FromBody] CreateClienteRequest request)
        {
            var result = await _clienteService.AgregarClienteAsync(
                request.Nombre,
                request.Apellido,
                request.Apodo,
                request.NIC,
                request.NumeroTelefono);

            return result switch
            {
                AgregarClienteResult.Success => Ok("Cliente agregado correctamente"),
                AgregarClienteResult.InvalidData => BadRequest("Los datos del cliente son invalidos"),
                AgregarClienteResult.NicAlreadyExists => Conflict("Ya existe un cliente con ese NIC"),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Ocurrio un error inesperado")
            };
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerClientes()
        {
            var clientes = await _clienteService.ObtenerClientesAsync();

            return Ok(clientes);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerClientePorId(int id)
        {
            var cliente = await _clienteService.ObtenerClientePorIdAsync(id);

            if (cliente == null)
            {
                return NotFound("Cliente no encontrado");
            }

            return Ok(cliente);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> EditarCliente(int id, [FromBody] UpdateClienteRequest request)
        {
            var result = await _clienteService.EditarClienteAsync(
                id,
                request.Nombre,
                request.Apellido,
                request.Apodo,
                request.NIC,
                request.NumeroTelefono );

            return result switch
            {
                EditarClienteResult.Success => NoContent(),
                EditarClienteResult.NicAlreadyExists => Conflict("Ya existe otro cliente con ese NIC"),
                EditarClienteResult.InvalidData => BadRequest("Los datos del cliente son invalidos"),
                EditarClienteResult.ClientNotFound => NotFound("No se pudo encontrar el cliente"),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Ocurrio un error inesperado")
            };

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> EliminarCliente(int id)
        {
            var result = await _clienteService.EliminarClienteAsync(id);

            return result switch
            {
                EliminarClienteResult.Success => NoContent(),
                EliminarClienteResult.ClientNotFound => NotFound("No se encontro el cliente"),
                EliminarClienteResult.ClientHasRecharges => Conflict("El cliente tiene recargas, no se pudo eliminar"),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Ocurrio un error inesperado")
            };
        }

        [HttpGet("search")]
        public async Task<ActionResult> BuscarClientes([FromQuery] string term)
        {
            var clientes = await _clienteService.BuscarClientesAsync(term);

            return Ok(clientes);
        }

    }
}
