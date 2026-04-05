

using Microsoft.EntityFrameworkCore;
using RechargeFunctions.Domain.Entities;
using RechargeFunctions.Domain.Enums.Recarga;

using RechargeFunctions.Infraestructure.Persistence;


namespace RechargeFunctions.Application.Services
{
    public class RecargaService
    {
        private readonly ApplicationDbContext _context;

        public RecargaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CrearRecargaResult> CrearRecargaAsync(int clienteId, int tarjetaId, decimal monto, bool estaPagada)
        {
            if ( monto <= 0)
            {
                return CrearRecargaResult.InvalidAmount;
            }

            var cliente = await _context.Clientes.FirstOrDefaultAsync(r => r.Id == clienteId);
            
            if (cliente == null) 
            {
                return CrearRecargaResult.ClientNotFound;
            }

            var tarjeta = await _context.Tarjetas.FirstOrDefaultAsync(r => r.Id == tarjetaId);

            if (tarjeta == null)
            {
                return CrearRecargaResult.CardNotFound;
            }

            if (!tarjeta.IsActive)
            {
                return CrearRecargaResult.CardInactive;
            }

            var recarga = new Recarga
            {
                ClienteId = clienteId,
                TarjetaId = tarjetaId,
                MontoRecarga = monto,
                EstaPagada = estaPagada,
                FechaPago = estaPagada ? DateTime.UtcNow : null
            };

            _context.Recargas.Add(recarga);
            await _context.SaveChangesAsync();

            return CrearRecargaResult.Success;
        }

        public async Task<EditarRecargaResult> EditarRecargaAsync(int id, int clienteId, int tarjetaId, decimal monto, bool estaPagada)
        {
            var recarga = await _context.Recargas.FirstOrDefaultAsync(r => r.Id == id);

            if (recarga == null)
            {
                return EditarRecargaResult.RechargeNotFound;
            }

            if (tarjetaId <= 0)
            {
                return EditarRecargaResult.InvalidCardId;
            }

            if (clienteId <= 0)
            {
                return EditarRecargaResult.InvalidClientId;
            }

            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == clienteId);

            if (cliente == null)
            {
                return EditarRecargaResult.ClientNotFound;
            }

            var tarjeta = await _context.Tarjetas.FirstOrDefaultAsync(t => t.Id == tarjetaId);

            if (tarjeta == null)
            {
                return EditarRecargaResult.CardNotFound;
            }

            if (!tarjeta.IsActive)
            {
                return EditarRecargaResult.CardInactive;
            }

            if (monto <= 0)
            {
                return EditarRecargaResult.InvalidAmount;
            }

            recarga.ClienteId = clienteId;
            recarga.TarjetaId = tarjetaId;
            recarga.MontoRecarga = monto;
            
            if (estaPagada)
            {
                if (!recarga.EstaPagada)
                {
                    recarga.FechaPago = DateTime.UtcNow;
                }
                
            }
            else
            {
                recarga.FechaPago = null;
            }

            recarga.EstaPagada = estaPagada;

            await _context.SaveChangesAsync();
            return EditarRecargaResult.Success;


        }

        public async Task<PagarRecargaResult> MarcarRecargaComoPagadaAsync(int id)
        {
            var recarga = await _context.Recargas.FirstOrDefaultAsync(r => r.Id == id);

            if (recarga == null)
            {
                return PagarRecargaResult.RechargeNotFound;
            }

            if (recarga.EstaPagada)
            {
                return PagarRecargaResult.RechargeAlreadyPaid;

            }

            recarga.EstaPagada = true;
            recarga.FechaPago = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return PagarRecargaResult.Success;
        }

        public async Task<EliminarRecargaResult> EliminarRecargaAsync(int id)
        {
            var recarga = await _context.Recargas.FirstOrDefaultAsync(r => r.Id == id);

            if (recarga == null)
            {
                return EliminarRecargaResult.RechargeNotFound;
            }

            if (recarga.IsDeleted)
            {
                return EliminarRecargaResult.IsAlreadyDeleted;
            }

            recarga.IsDeleted = true;

            await _context.SaveChangesAsync();
            return EliminarRecargaResult.Success;
        }

        public async Task<List<Recarga>> ObtenerRecargasAsync()
        {
            var recargas = await _context.Recargas
                .AsNoTracking()
                .ToListAsync();

            return recargas;
        }

        public async Task<Recarga?> ObtenerRecargaPorIdAsync(int id)
        {
            var recarga = await _context.Recargas
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            return recarga;
        }

    }
}
