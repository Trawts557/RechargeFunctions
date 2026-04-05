namespace RechargeFunctions.Domain.Entities
{
    public class Tarjeta
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string UltimosDigitos { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public ICollection<Recarga> Recargas { get; set; } = new List<Recarga>();
    }
}
