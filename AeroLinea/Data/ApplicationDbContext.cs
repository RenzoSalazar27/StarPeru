using AeroLinea.Models;
using Microsoft.EntityFrameworkCore;

namespace AeroLinea.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Pasajero> Pasajeros { get; set; }
        public DbSet<RegisteredFlight> RegisteredFlights { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<RoutePrice> RoutePrices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pasajero>(entity =>
            {
                entity.ToTable("iam_account");
                
                entity.HasKey(e => e.id);

                entity.Property(e => e.id)
                    .HasColumnName("id")
                    .HasColumnType("bigint")
                    .ValueGeneratedOnAdd();
                
                entity.Property(e => e.sender_id)
                    .IsRequired()
                    .HasColumnName("sender_id")
                    .HasColumnType("varchar(8)");

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.lastname)
                    .IsRequired()
                    .HasColumnName("lastname")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(1000)");

                entity.Property(e => e.password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(1000)");

                entity.Property(e => e.flag_admin)
                    .HasColumnName("flag_admin")
                    .HasColumnType("int");

                entity.Property(e => e.flag_active)
                    .HasColumnName("flag_active")
                    .HasColumnType("int");

                entity.Property(e => e.created_at)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.deleted_at)
                    .HasColumnName("deleted_at")
                    .HasColumnType("timestamp")
                    .IsRequired(false);
            });

            modelBuilder.Entity<RegisteredFlight>(entity =>
            {
                entity.ToTable("registered_flights");
                
                entity.HasKey(e => e.id);

                entity.Property(e => e.id)
                    .HasColumnName("id")
                    .HasColumnType("int")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.number)
                    .HasColumnName("number")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.type_fly_id)
                    .HasColumnName("type_fly_id")
                    .HasColumnType("int");

                entity.Property(e => e.origin_id)
                    .HasColumnName("origin_id")
                    .HasColumnType("int");

                entity.Property(e => e.destiny_id)
                    .HasColumnName("destiny_id")
                    .HasColumnType("int");

                entity.Property(e => e.time_start)
                    .HasColumnName("time_start")
                    .HasColumnType("datetime");

                entity.Property(e => e.flag_active)
                    .HasColumnName("flag_active")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.created_at)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.deleted_at)
                    .HasColumnName("deleted_at")
                    .HasColumnType("datetime")
                    .IsRequired(false);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("locations");
                
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(100)")
                    .IsRequired();

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasColumnType("varchar(10)")
                    .IsRequired();

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasColumnType("bit")
                    .HasDefaultValue(true);
            });
        }
    }
}
