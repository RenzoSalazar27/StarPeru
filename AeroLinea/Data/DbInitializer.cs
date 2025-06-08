using AeroLinea.Models;
using Microsoft.EntityFrameworkCore;

namespace AeroLinea.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Verificar si ya hay ubicaciones
            if (!context.Locations.Any())
            {
                var locations = new Location[]
                {
                    new Location { Name = "Lima", Code = "LIM", IsActive = true },
                    new Location { Name = "Iquitos", Code = "IQT", IsActive = true },
                    new Location { Name = "Tarapoto", Code = "TPP", IsActive = true },
                    new Location { Name = "Huánuco", Code = "HUU", IsActive = true },
                    new Location { Name = "Pucallpa", Code = "PCL", IsActive = true },
                    new Location { Name = "Cajamarca", Code = "CJA", IsActive = true },
                    new Location { Name = "Chiclayo", Code = "CIX", IsActive = true }
                };

                context.Locations.AddRange(locations);
                context.SaveChanges();
            }
        }
    }
} 