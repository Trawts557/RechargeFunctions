namespace RechargeFunctions.Domain.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        public string NIC { get; set; } = string.Empty;
        public string? Apodo { get; set; } 
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? NumeroTelefono { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<Recarga> Recargas { get; set; } = new List<Recarga>();
    }
}
