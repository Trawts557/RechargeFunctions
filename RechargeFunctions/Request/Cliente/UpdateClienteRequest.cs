namespace RechargeFunctions.Api.Request.Cliente
{
    public class UpdateClienteRequest
    {
        public string? Nombre { get; set; } = string.Empty;
        public string? Apellido { get; set; } = string.Empty;
        public string? Apodo { get; set; }
        public string NIC { get; set; } = string.Empty;
        public string? NumeroTelefono { get; set; }
    }
}
