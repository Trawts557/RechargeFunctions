namespace RechargeFunctions.Mobile.Models.Tarjeta
{
    public class ResumenTarjetaDto
    {
        public int TarjetaId { get; set; }
        public string TarjetaNombre { get; set; } = string.Empty;
        public decimal TotalGastado { get; set; }
        public int CantidadRecargas { get; set; }
        public int CantidadPagadas { get; set; }
        public int CantidadPendientes { get; set; }
        public decimal Ganancias { get; set; }
    }
}