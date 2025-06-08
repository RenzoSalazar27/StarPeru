using AeroLinea.Data;
using AeroLinea.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using AeroLinea.Filters;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
namespace AeroLinea.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; 


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index() //vista de la pestaña principal
        {
            // Cargar las ubicaciones para los dropdowns
            ViewBag.Origins = _context.Locations.ToList();
            ViewBag.Destinies = _context.Locations.ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Apartir de aquí se mostraran las opciones del menú a la izquierda del logo, osea el iconito con forma de "hamburguesa" :v

        public IActionResult Autenticacion()
        {
            return View("Pasajero/Autenticacion");
        }

        public IActionResult registroPasajero()
        {
            return View("Pasajero/registroPasajero");
        }

        public IActionResult Alianzas() // Opción para abrir "Alianzas"
        {
            return View(); // Vista "Alianzas.cshtml"
        }

        public IActionResult Promociones() // Opción para abrir "Promociones"
        {
            return View(); // Vista "Promociones.cshtml"
        }

        public IActionResult Servicios_al_cliente()
        {
            return View();
        }

        public IActionResult Destinos()
        {
            return View();
        }

        public IActionResult Oficinas()
        {
            return View();
        }

        public IActionResult Flota()
        {
            return View();
        }

        public IActionResult Reserva()
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Autenticacion");
            }

            return View();
        }

        public IActionResult Equipaje()
        {
            return View();
        }

        public IActionResult Cargo()
        {
            return View();
        }

        public IActionResult Vuela()
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Autenticacion");
            }

            // Cargar las ubicaciones para los dropdowns
            ViewBag.Origins = _context.Locations.ToList();
            ViewBag.Destinies = _context.Locations.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult ReservarVuelo(int origin_id, int destiny_id, DateTime time_start, string flag_active)
        {
            try
            {
                if (HttpContext.Session.GetString("UserEmail") == null)
                {
                    return RedirectToAction("Autenticacion");
                }

                var reserva = new RegisteredFlight
                {
                    origin_id = origin_id,
                    destiny_id = destiny_id,
                    time_start = time_start,
                    flag_active = flag_active,
                    created_at = DateTime.Now
                };

                _context.RegisteredFlights.Add(reserva);
                _context.SaveChanges();

                TempData["Success"] = "Vuelo reservado exitosamente";
                return RedirectToAction("Vuela");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al reservar el vuelo. Por favor, intente nuevamente.";
                return RedirectToAction("Vuela");
            }
        }
        
        [HttpPost]
        public IActionResult registroPasajero(Pasajero pasajero)
        {
            try
            {
                _logger.LogInformation("=== INICIO DEL REGISTRO DE PASAJERO ===");

                // Establecer valores por defecto
                pasajero.flag_admin = 0;
                pasajero.flag_active = 1;
                pasajero.created_at = DateTime.Now;
                pasajero.deleted_at = null;

                // Imprimir todos los valores antes de guardar
                _logger.LogInformation($@"Datos a guardar:
                    ID: {pasajero.id}
                    Email: {pasajero.email}
                    Nombre: {pasajero.name}
                    Apellido: {pasajero.lastname}
                    DNI: {pasajero.sender_id}
                    Password: {pasajero.password}
                    FlagAdmin: {pasajero.flag_admin}
                    FlagActive: {pasajero.flag_active}
                    CreatedAt: {pasajero.created_at}
                    DeletedAt: {pasajero.deleted_at}");

                // Guardar en la base de datos
                _context.Pasajeros.Add(pasajero);
                
                try {
                    _logger.LogInformation("Intentando guardar en la base de datos...");
                    
                    // Verificar el estado de la entidad
                    var entry = _context.Entry(pasajero);
                    _logger.LogInformation($"Estado de la entidad: {entry.State}");
                    
                    var saveResult = _context.SaveChanges();
                    _logger.LogInformation($"SaveChanges retornó: {saveResult} registros afectados");
                    
                    if (saveResult > 0) {
                        _logger.LogInformation($"Registro guardado exitosamente con ID: {pasajero.id}");
                        
                        // Verificar si el registro existe en la base de datos
                        var savedUser = _context.Pasajeros.FirstOrDefault(p => p.email == pasajero.email);
                        if (savedUser != null)
                        {
                            _logger.LogInformation($"Verificación: Usuario encontrado en la base de datos con ID: {savedUser.id}");
                        }
                        else
                        {
                            _logger.LogError("Verificación: ¡El usuario no se encuentra en la base de datos después de guardar!");
                        }
                        
                        TempData["Success"] = "Registro exitoso. Por favor, inicia sesión.";
                        return RedirectToAction("Autenticacion");
                    } else {
                        _logger.LogError("SaveChanges retornó 0, no se guardó el registro");
                        ModelState.AddModelError("", "No se pudo guardar el registro. Por favor, intenta nuevamente.");
                        return View("Pasajero/registroPasajero", pasajero);
                    }
                } catch (DbUpdateException dbEx) {
                    _logger.LogError($"Error de base de datos al guardar: {dbEx.Message}");
                    if (dbEx.InnerException != null) {
                        _logger.LogError($"Error interno de base de datos: {dbEx.InnerException.Message}");
                    }
                    ModelState.AddModelError("", "Error al guardar en la base de datos. Por favor, intenta nuevamente.");
                    return View("Pasajero/registroPasajero", pasajero);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error no controlado: {ex.Message}");
                if (ex.InnerException != null)
                {
                    _logger.LogError($"Error interno no controlado: {ex.InnerException.Message}");
                }
                ModelState.AddModelError("", "Error al registrar. Por favor, intenta nuevamente.");
                return View("Pasajero/registroPasajero", pasajero);
            }
        }

        [TypeFilter(typeof(AdminAuthFilter))]
        public IActionResult panelAdministrador()
        {
            if (HttpContext.Session.GetString("AdminUsername") == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View("Administrador/panelAdministrador");
        }

        public IActionResult SelectLogin()
        {
            return View();
        }

        public IActionResult LogoutUser()
        {
            HttpContext.Session.Remove("UserEmail");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult LoginUser(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TempData["Error"] = "Por favor, ingresa tu email y contraseña";
                return RedirectToAction("Autenticacion");
            }

            try
            {
                _logger.LogInformation($"Intentando iniciar sesión con email: {email}");
                
                // Primero buscar el usuario solo por email
                var user = _context.Pasajeros.FirstOrDefault(p => p.email == email);
                
                if (user == null)
                {
                    _logger.LogWarning($"Usuario no encontrado con email: {email}");
                    TempData["Error"] = "Email o contraseña incorrectos";
                    return RedirectToAction("Autenticacion");
                }

                _logger.LogInformation($"Usuario encontrado - ID: {user.id}, Email: {user.email}, FlagActive: {user.flag_active}");

                if (user.flag_active != 1)
                {
                    _logger.LogWarning($"Usuario inactivo - ID: {user.id}, Email: {user.email}, FlagActive: {user.flag_active}");
                    TempData["Error"] = "Esta cuenta está inactiva. Por favor, contacta al administrador.";
                    return RedirectToAction("Autenticacion");
                }

                // Verificar la contraseña
                _logger.LogInformation($"Contraseña proporcionada: {password}");
                _logger.LogInformation($"Contraseña almacenada: {user.password}");
                
                if (user.password == password)
                {
                    _logger.LogInformation($"Inicio de sesión exitoso para el usuario: {email}");
                    HttpContext.Session.SetString("UserEmail", user.email);
                    HttpContext.Session.SetString("UserName", user.name);
                    return RedirectToAction("Index");
                }

                _logger.LogWarning($"Contraseña incorrecta para el usuario: {email}");
                TempData["Error"] = "Email o contraseña incorrectos";
                return RedirectToAction("Autenticacion");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al intentar iniciar sesión: {ex.Message}");
                if (ex.InnerException != null)
                {
                    _logger.LogError($"Error interno: {ex.InnerException.Message}");
                }
                TempData["Error"] = "Error al intentar iniciar sesión. Por favor, intenta nuevamente.";
                return RedirectToAction("Autenticacion");
            }
        }

        [HttpPost]
        public IActionResult ProcesarReserva(string tipoAvion, string tipoVuelo, string tipoAsiento)
        {
            try
            {
                if (HttpContext.Session.GetString("UserEmail") == null)
                {
                    return RedirectToAction("Autenticacion");
                }

                // Obtener los datos de la sesión
                var origin_id = HttpContext.Session.GetInt32("SelectedOriginId");
                var destiny_id = HttpContext.Session.GetInt32("SelectedDestinyId");
                var time_start = DateTime.Parse(HttpContext.Session.GetString("SelectedDate"));

                var reserva = new RegisteredFlight
                {
                    origin_id = origin_id.Value,
                    destiny_id = destiny_id.Value,
                    time_start = time_start,
                    number = GenerateFlightNumber(),
                    type_fly_id = GetTipoVueloId(tipoVuelo),
                    flag_active = tipoAsiento,
                    created_at = DateTime.Now
                };

                _context.RegisteredFlights.Add(reserva);
                _context.SaveChanges();

                TempData["Success"] = "Vuelo reservado exitosamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la reserva: {ex.Message}");
                TempData["Error"] = "Error al procesar la reserva. Por favor, intente nuevamente.";
                return RedirectToAction("Reserva");
            }
        }

        private string GenerateFlightNumber()
        {
            return $"SP{DateTime.Now.ToString("yyyyMMddHHmm")}";
        }

        private int GetTipoVueloId(string tipoVuelo)
        {
            return tipoVuelo.ToLower() == "internacional" ? 2 : 1;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GuardarSeleccionVuelo([FromBody] VueloSeleccion seleccion)
        {
            try
            {
                _logger.LogInformation($"Valores recibidos: origin_id={seleccion.origin_id}, destiny_id={seleccion.destiny_id}, time_start={seleccion.time_start}");

                // Validar que los IDs sean válidos
                if (seleccion.origin_id <= 0 || seleccion.destiny_id <= 0)
                {
                    _logger.LogWarning($"IDs inválidos: origin_id={seleccion.origin_id}, destiny_id={seleccion.destiny_id}");
                    return Json(new { success = false, message = "Los IDs de origen y destino deben ser válidos" });
                }

                // Validar que origen y destino sean diferentes
                if (seleccion.origin_id == seleccion.destiny_id)
                {
                    _logger.LogWarning("Origen y destino son iguales");
                    return Json(new { success = false, message = "El origen y destino no pueden ser iguales" });
                }

                // Validar que los IDs existan en la base de datos
                var originExists = _context.Locations.Any(l => l.Id == seleccion.origin_id);
                var destinyExists = _context.Locations.Any(l => l.Id == seleccion.destiny_id);

                _logger.LogInformation($"Existencia en base de datos: origin={originExists}, destiny={destinyExists}");

                if (!originExists || !destinyExists)
                {
                    _logger.LogWarning("Las ubicaciones no existen en la base de datos");
                    return Json(new { success = false, message = "Las ubicaciones seleccionadas no son válidas" });
                }

                // Guardar selección en la sesión
                HttpContext.Session.SetInt32("SelectedOriginId", seleccion.origin_id);
                HttpContext.Session.SetInt32("SelectedDestinyId", seleccion.destiny_id);
                HttpContext.Session.SetString("SelectedDate", seleccion.time_start.ToString("o"));

                _logger.LogInformation("Selección guardada exitosamente en la sesión");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la selección: {ex.Message}");
                return Json(new { success = false, message = "Error al procesar la selección. Por favor, intente nuevamente." });
            }
        }

        public class VueloSeleccion
        {
            public int origin_id { get; set; }
            public int destiny_id { get; set; }
            public DateTime time_start { get; set; }
        }

        public IActionResult SeleccionClase()
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Autenticacion");
            }

            // Verificar si hay datos de vuelo en la sesión
            if (HttpContext.Session.GetInt32("SelectedOriginId") == null ||
                HttpContext.Session.GetInt32("SelectedDestinyId") == null ||
                HttpContext.Session.GetString("SelectedDate") == null)
            {
                return RedirectToAction("Vuela");
            }

            return View();
        }

        [HttpPost]
        public IActionResult GetRoutePrice(int originId, int destinyId)
        {
            try
            {
                var routePrice = _context.RoutePrices
                    .FirstOrDefault(r => r.OriginId == originId && r.DestinyId == destinyId);

                if (routePrice == null)
                {
                    return Json(new { success = false, message = "Ruta no disponible" });
                }

                return Json(new
                {
                    success = true,
                    prices = new
                    {
                        economy = routePrice.EconomyPrice,
                        executive = routePrice.ExecutivePrice,
                        premium = routePrice.PremiumPrice
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener precios de ruta: {ex.Message}");
                return Json(new { success = false, message = "Error al obtener precios" });
            }
        }
        public IActionResult PagoFinal()
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Autenticacion");
            }

            ViewBag.Email = HttpContext.Session.GetString("UserEmail");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcesarPago(string metodoPago, string numeroTarjeta, string nombreTarjeta, string fechaExpiracion,
     string cvv, string email, string celular, string tipoDocumento, string bancoTarjeta, string precioConDescuento)
        {
            try
            {
                // Puedes agregar aquí validaciones extra si lo deseas

                decimal precioFinal = decimal.Parse(precioConDescuento);

                TempData["Success"] = $"Pago realizado exitosamente con {bancoTarjeta}. Monto final: S/. {precioFinal:F2}. Se generó su {tipoDocumento.ToUpper()}.";

                return RedirectToAction("ConfirmacionReserva");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en el pago: {ex.Message}");
                TempData["Error"] = "Error al procesar el pago. Intente nuevamente.";
                return RedirectToAction("PagoFinal");
            }
        }
        public IActionResult ConfirmacionReserva()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (userEmail == null)
                return RedirectToAction("Autenticacion");

            // Simula datos de la reserva. En una app real, puedes traerlos desde la BD o sesión.
            string nombre = HttpContext.Session.GetString("UserName") ?? "Pasajero";
            string banco = HttpContext.Session.GetString("MetodoPago") ?? "Tarjeta Desconocida";
            string tipoDoc = HttpContext.Session.GetString("TipoDocumento") ?? "Boleta";
            string monto = HttpContext.Session.GetString("MontoTotal") ?? "S/. 0.00";

            // Crear documento PDF
            var documento = new PdfDocument();
            var pagina = documento.AddPage();
            var gfx = XGraphics.FromPdfPage(pagina);
            var fuente = new XFont("Arial", 14, XFontStyle.Regular);

            gfx.DrawString("Aerolínea - Confirmación de " + tipoDoc, new XFont("Arial", 18, XFontStyle.Bold), XBrushes.Black,
                           new XRect(0, 20, pagina.Width, 40), XStringFormats.TopCenter);

            gfx.DrawString($"Nombre: {nombre}", fuente, XBrushes.Black, new XPoint(50, 80));
            gfx.DrawString($"Correo: {userEmail}", fuente, XBrushes.Black, new XPoint(50, 110));
            gfx.DrawString($"Método de Pago: {banco}", fuente, XBrushes.Black, new XPoint(50, 140));
            gfx.DrawString($"Tipo de Documento: {tipoDoc}", fuente, XBrushes.Black, new XPoint(50, 170));
            gfx.DrawString($"Monto Pagado: {monto}", fuente, XBrushes.Black, new XPoint(50, 200));
            gfx.DrawString($"Fecha de Emisión: {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}", fuente, XBrushes.Black, new XPoint(50, 230));

            // Guardar en memoria y devolver como archivo
            using var stream = new MemoryStream();
            documento.Save(stream, false);
            return File(stream.ToArray(), "application/pdf", $"Confirmacion_{tipoDoc}_{DateTime.Now:yyyyMMddHHmmss}.pdf");
        }

    }
}
