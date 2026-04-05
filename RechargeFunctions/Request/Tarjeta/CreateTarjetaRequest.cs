namespace RechargeFunctions.Api.Request.Cliente
{
    public class CreateTarjetaRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string UltimosDigitos { get; set; } = string.Empty;
    }
}
