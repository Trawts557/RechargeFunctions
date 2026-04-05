namespace RechargeFunctions.Mobile.Models.Cliente
{
    public class ClienteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Apodo { get; set; } = string.Empty;
        public string Nic { get; set; } = string.Empty;
        public string NumeroTelefono { get; set; } = string.Empty;

        public string NombreCompleto
        {
            get
            {
                return $"{Nombre} {Apellido}";
            }
        }

        public string DisplayText
        {
            get
            {
                return $"{Nombre} {Apellido} - {Apodo}";
            }
        }
    }
}