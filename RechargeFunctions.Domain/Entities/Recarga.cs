namespace RechargeFunctions.Domain.Entities
{
    public class Recarga
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;

        public int TarjetaId { get; set; }
        public Tarjeta Tarjeta { get; set; } = null!;

        public bool EstaPagada { get; set; }
        public bool IsDeleted { get; set; }
        public decimal MontoRecarga { get; set; }
        public DateTime FechaRecarga { get; set; } = DateTime.UtcNow;
        public DateTime? FechaPago { get; set; }
    }
}
