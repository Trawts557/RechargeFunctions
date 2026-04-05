namespace RechargeFunctions.Mobile.Models.Recarga
{
    public class RecargaDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int TarjetaId { get; set; }
        public decimal MontoRecarga { get; set; }
        public bool EstaPagada { get; set; }
        public DateTime FechaRecarga { get; set; }
        public DateTime? FechaPago { get; set; }

        public string ClienteNombre { get; set; } = string.Empty;
        public string TarjetaNombre { get; set; } = string.Empty;

        public string EstadoTexto
        {
            get
            {
                return EstaPagada ? "Pagada" : "Pendiente";
            }
        }
    }
}