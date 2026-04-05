
using Microsoft.EntityFrameworkCore;
using RechargeFunctions.Domain.Entities;
using RechargeFunctions.Domain.Enums.Tarjeta;
using RechargeFunctions.Infraestructure.Persistence;

namespace RechargeFunctions.Application.Services
{
    public class TarjetaService
    {
        private readonly ApplicationDbContext _context;

        public TarjetaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CrearTarjetaResult> AgregarTarjetaAsync(string nombre, string ultimosDigitos)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(ultimosDigitos))
            {
                return CrearTarjetaResult.InvalidData;
            }

            if (ultimosDigitos.Length < 2)
            {
                return CrearTarjetaResult.InvalidLastDigits;
            }

            var ultimosDigitosEnUso = await _context.Tarjetas
                .AnyAsync(t => t.UltimosDigitos == ultimosDigitos);

            if (ultimosDigitosEnUso)
            {
                return CrearTarjetaResult.CardAlreadyExists;
            }

            var tarjeta = new Tarjeta
            {
                Nombre = nombre,
                UltimosDigitos = ultimosDigitos,
                IsActive = true
            };



            _context.Tarjetas.Add(tarjeta);

            await _context.SaveChangesAsync();
            return CrearTarjetaResult.Success;
        }

        public async Task<EditarTarjetaResult> EditarTarjetaAsync(int id, string nombre, string ultimosDigitos)
        {
            var tarjeta = await _context.Tarjetas.FirstOrDefaultAsync(t => t.Id == id);
            
            if (tarjeta == null)
            {
                return EditarTarjetaResult.CardNotFound;
            }

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(ultimosDigitos))
            {
                return EditarTarjetaResult.InvalidData;
            }

            if (ultimosDigitos.Length < 2)
            {
                return EditarTarjetaResult.InvalidLastDigits;
            }

            var ultimosDigitosEnUso = await _context.Tarjetas
                .AnyAsync(t => t.UltimosDigitos == ultimosDigitos);

            if (ultimosDigitosEnUso)
            {
                return EditarTarjetaResult.CardAlreadyExists;
            }

            tarjeta.Nombre = nombre;
            tarjeta.UltimosDigitos = ultimosDigitos;

            await _context.SaveChangesAsync();
            return EditarTarjetaResult.Success;
        }

        public async Task<EliminarTarjetaResult> DesactivarTarjetaAsync(int id)
        {
            var tarjeta = await _context.Tarjetas.FirstOrDefaultAsync(t => t.Id == id);

            if (tarjeta == null)
            {
                return EliminarTarjetaResult.CardNotFound;
            }

            if (!tarjeta.IsActive)
            {
                return EliminarTarjetaResult.IsAlreadyDeleted;
            }

            tarjeta.IsActive = false;

            await _context.SaveChangesAsync();
            return EliminarTarjetaResult.Success;

        }

        public async Task<ActivarTarjetaResult> ActivarTarjetaAsync(int id)
        {
            var tarjeta = await _context.Tarjetas.FirstOrDefaultAsync(t => t.Id == id);

            if (tarjeta == null)
            {
                return ActivarTarjetaResult.CardNotFound;
            }

            if (tarjeta.IsActive)
            {
                return ActivarTarjetaResult.CardIsAlreadyActive;
            }

            tarjeta.IsActive = true;

            await _context.SaveChangesAsync();
            return ActivarTarjetaResult.Success;

        }

        public async Task<List<Tarjeta>> ObtenerTarjetasAsync()
        {
            var tarjetas = await _context.Tarjetas
                .AsNoTracking()
                .ToListAsync();

            return tarjetas;
        }

        public async Task<Tarjeta?> ObtenerTarjetaPorIdAsync(int id)
        {
            var tarjeta = await _context.Tarjetas
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
                       
            return tarjeta;
        }

    }
}
