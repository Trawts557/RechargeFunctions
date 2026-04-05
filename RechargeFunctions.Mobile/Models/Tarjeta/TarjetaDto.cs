namespace RechargeFunctions.Mobile.Models.Tarjeta
{
    public class TarjetaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string UltimosDigitos { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public string DisplayText
        {
            get
            {
                return $"{Nombre} - {UltimosDigitos}";
            }
        }

        public string EstadoTexto
        {
            get
            {
                return IsActive ? "Activa" : "Inactiva";
            }
        }
    }
}