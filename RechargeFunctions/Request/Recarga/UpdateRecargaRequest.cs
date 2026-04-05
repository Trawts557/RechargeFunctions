namespace RechargeFunctions.Api.Request.Recarga
{
    public class UpdateRecargaRequest
    {
        public int TarjetaId { get; set; }
        public int ClienteId { get; set; }
        public decimal Monto { get; set; }
        public bool EstaPagada { get; set; }

    }
}
