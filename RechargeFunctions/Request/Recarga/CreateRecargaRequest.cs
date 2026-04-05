namespace RechargeFunctions.Api.Request.Recarga
{
    public class CreateRecargaRequest
    {
        public int ClienteId { get; set; }
        public int TarjetaId { get; set; }
        public decimal MontoRecarga { get; set; }
        public bool EstaPagada { get; set; }

    }
}
