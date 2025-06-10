using Microsoft.EntityFrameworkCore;
using AeroLinea.Models;

namespace Data
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
    }
} 