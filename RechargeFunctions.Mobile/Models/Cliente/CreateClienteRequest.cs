namespace RechargeFunctions.Mobile.Models.Cliente
{
    public class CreateClienteRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Apodo { get; set; } = string.Empty;
        public string Nic { get; set; } = string.Empty;
        public string NumeroTelefono { get; set; } = string.Empty;
    }
}