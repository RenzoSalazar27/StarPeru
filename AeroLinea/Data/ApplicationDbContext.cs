using Microsoft.EntityFrameworkCore;
using AeroLinea.Models;

namespace AeroLinea.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Piloto> Pilotos { get; set; }
        public DbSet<Flota> Flota { get; set; }
        public DbSet<Vuelo> Vuelo { get; set; }
        public DbSet<ReservaVuelo> ReservaVuelos { get; set; }
        public DbSet<Pasajero> Pasajeros { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Reserva)
                .WithMany()
                .HasForeignKey(p => p.IdReserva)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 