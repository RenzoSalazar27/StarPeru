using Microsoft.EntityFrameworkCore;
using AeroLinea.Models;
using AeroLinea.Services;
using AeroLinea.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configurar el DbContext con MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Cadena de conexión: {connectionString}");

builder.Services.AddDbContext<AeroLinea.Data.ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
        mySqlOptions =>
        {
            mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        })
    .LogTo(Console.WriteLine, LogLevel.Information)
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors();
});

builder.Services.AddScoped<StripeService>();

var app = builder.Build();

// Asegurar que la base de datos existe y está actualizada
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        Console.WriteLine("Intentando conectar a la base de datos...");
        var context = services.GetRequiredService<AeroLinea.Data.ApplicationDbContext>();
        
        Console.WriteLine("Verificando si la base de datos existe...");
        if (context.Database.CanConnect())
        {
            Console.WriteLine("Conexión exitosa a la base de datos");
            Console.WriteLine("Datos inicializados correctamente");
        }
        else
        {
            Console.WriteLine("No se pudo conectar a la base de datos");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al conectar con la base de datos: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Error interno: {ex.InnerException.Message}");
        }
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al crear/actualizar la base de datos.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// Configurar el manejo de errores en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.Run();
