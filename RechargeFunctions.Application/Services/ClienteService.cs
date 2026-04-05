
using Microsoft.EntityFrameworkCore;
using RechargeFunctions.Domain.Entities;
using RechargeFunctions.Domain.Enums.Cliente;
using RechargeFunctions.Infraestructure.Persistence;

namespace RechargeFunctions.Application.Services
{
    public class ClienteService
    {
        private readonly ApplicationDbContext _context;

        public ClienteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AgregarClienteResult> AgregarClienteAsync(string nombre, string apellido, string? apodo, string nic, string? numeroTelefono)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return AgregarClienteResult.InvalidData;
            }

            if (string.IsNullOrWhiteSpace(apellido))
            {
                return AgregarClienteResult.InvalidData;
            }

            if (string.IsNullOrWhiteSpace(nic))
            {
                return AgregarClienteResult.InvalidData;
            }

            var nicDuplicado = await _context.Clientes.AnyAsync(c => c.NIC == nic);

            if (nicDuplicado)
            {
                return AgregarClienteResult.NicAlreadyExists;
            }

            var cliente = new Cliente
            {
                Nombre = nombre,
                Apellido = apellido,
                Apodo = apodo,
                NIC = nic,
                NumeroTelefono = numeroTelefono
            };

            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();

            return AgregarClienteResult.Success;
         
        }

        public async Task<EliminarClienteResult> EliminarClienteAsync(int id)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
            if (cliente == null) 
            {
                return EliminarClienteResult.ClientNotFound;
            }

            var tieneRecargas = await _context.Recargas.AnyAsync(r => r.ClienteId == id);

            if (tieneRecargas) 
            {
                return EliminarClienteResult.ClientHasRecharges;
            }

            cliente.IsDeleted = true;

            await _context.SaveChangesAsync();

            return EliminarClienteResult.Success;

        }

        public async Task<EditarClienteResult> EditarClienteAsync(int id, string nombre, string apellido, string apodo, string nic, string numeroTelefono)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
            
            if (cliente == null)
            {
                return EditarClienteResult.ClientNotFound;
            }

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido) 
                || string.IsNullOrWhiteSpace(nic) )
            {
                return EditarClienteResult.InvalidData;
            }

            var nicDuplicado = await _context.Clientes.AnyAsync(c => c.NIC == nic && c.Id != id);

            if (nicDuplicado)
            {
                return EditarClienteResult.NicAlreadyExists;
            }

            cliente.Nombre = nombre;
            cliente.Apellido = apellido;
            cliente.Apodo = apodo;
            cliente.NIC = nic;
            cliente.NumeroTelefono = numeroTelefono;

            await _context.SaveChangesAsync();

            return EditarClienteResult.Success;
        }

        public async Task<List<Cliente>> BuscarClientesAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return new List<Cliente>();
            }

            
            term = term.Trim();

            var clientes = await _context.Clientes
                .AsNoTracking()
                .Where(c => 
                c.NIC.Contains(term) || 
                c.Nombre.Contains(term) ||
                c.Apellido.Contains(term) ||
                (c.Apodo != null && c.Apodo.Contains(term))
                )
                .OrderBy(c => c.Nombre)
                .Take(10)
                .ToListAsync();

            return clientes;
        }

        public async Task<List<Cliente>> ObtenerClientesAsync()
        {
            var clientes = await _context.Clientes
                .AsNoTracking()
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            return clientes;

        }

        public async Task<Cliente?> ObtenerClientePorIdAsync(int id)
        {
            var cliente = await _context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            return cliente;

        }

    }
}
