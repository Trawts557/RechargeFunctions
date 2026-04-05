

using Microsoft.EntityFrameworkCore;
using RechargeFunctions.Domain.Entities;

namespace RechargeFunctions.Infraestructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Tarjeta> Tarjetas { get; set; }
        public DbSet<Recarga> Recargas { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>().HasIndex(c => c.NIC).IsUnique();

            modelBuilder.Entity<Cliente>().HasQueryFilter(c => !c.IsDeleted);

            modelBuilder.Entity<Recarga>()
                .HasOne(r => r.Cliente)
                .WithMany(c => c.Recargas)
                .HasForeignKey(r => r.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Recarga>()
                .HasOne(r => r.Tarjeta)
                .WithMany(t => t.Recargas)
                .HasForeignKey(r => r.TarjetaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

/* ModelBuilder para evitar warning
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Cliente>()
        .HasIndex(c => c.NIC)
        .IsUnique();

    modelBuilder.Entity<Cliente>()
        .HasQueryFilter(c => !c.IsDeleted);

    modelBuilder.Entity<Recarga>()
        .HasQueryFilter(r => !r.IsDeleted);

    modelBuilder.Entity<Recarga>()
        .Property(r => r.MontoRecarga)
        .HasPrecision(18, 2);

    modelBuilder.Entity<Recarga>()
        .HasOne(r => r.Cliente)
        .WithMany(c => c.Recargas)
        .HasForeignKey(r => r.ClienteId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Recarga>()
        .HasOne(r => r.Tarjeta)
        .WithMany(t => t.Recargas)
        .HasForeignKey(r => r.TarjetaId)
        .OnDelete(DeleteBehavior.Restrict);
}
*/