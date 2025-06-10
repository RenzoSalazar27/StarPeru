using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AeroLinea.Models;
using System.Diagnostics;
using Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace AeroLinea.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Data.ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, Data.ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
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

        public IActionResult Login()
        {
            return View("~/Views/Autenticacion/Autenticacion.cshtml");
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            try
            {
                var usuario = _context.Usuarios.FirstOrDefault(u => u.correoUsuario == email && u.contrasenaUsuario == password);
                if (usuario != null)
                {
                    HttpContext.Session.SetString("UserEmail", usuario.correoUsuario);
                    HttpContext.Session.SetString("UserName", usuario.nombresUsuario);
                    HttpContext.Session.SetString("UserType", usuario.esAdmin ? "admin" : "user");
                    HttpContext.Session.SetInt32("UserId", usuario.idUsuario);
                    
                    // Asegurarse de que la sesión se guarde
                    HttpContext.Session.CommitAsync().Wait();
                    
                    return RedirectToAction("Index", "Home");
                }
                TempData["AuthError"] = "El correo o contraseña no son correctos";
            }
            catch (Exception ex)
            {
                TempData["AuthError"] = "Error al iniciar sesión: " + ex.Message;
            }
            return View("~/Views/Autenticacion/Autenticacion.cshtml");
        }

        public IActionResult Registro()
        {
            return View("~/Views/Autenticacion/registroPasajero.cshtml");
        }

        [HttpPost]
        public IActionResult registroPasajero(Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    usuario.esAdmin = false;
                    var usuarioExistente = _context.Usuarios.FirstOrDefault(u => u.correoUsuario == usuario.correoUsuario);
                    if (usuarioExistente != null)
                    {
                        TempData["Error"] = "El correo electrónico ya está registrado";
                        return View("~/Views/Autenticacion/registroPasajero.cshtml", usuario);
                    }

                    _context.Usuarios.Add(usuario);
                    _context.SaveChanges();

                    HttpContext.Session.SetString("UserEmail", usuario.correoUsuario);
                    HttpContext.Session.SetString("UserName", usuario.nombresUsuario);
                    HttpContext.Session.SetString("UserType", "user");
                    HttpContext.Session.SetInt32("UserId", usuario.idUsuario);

                    TempData["Success"] = "Registro exitoso";
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    TempData["Error"] = error.ErrorMessage;
                    break;
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al registrar: " + ex.Message;
            }

            return View("~/Views/Autenticacion/registroPasajero.cshtml", usuario);
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // Acciones para las páginas estáticas
        public IActionResult Destinos() => View("~/Views/Home/Destinos/Destinos.cshtml");

        public IActionResult VuelosPorDestino(string destino)
        {
            var vuelos = _context.Vuelo
                .Where(v => v.destinoVuelo == destino)
                .ToList();
            
            ViewBag.Destino = destino;
            return PartialView("~/Views/Home/Destinos/vuelosDestinos.cshtml", vuelos);
        }

        public IActionResult VuelosEconomicos()
        {
            var vuelos = _context.Vuelo
                .Where(v => v.precioVuelo < 300.00m)
                .ToList();
            
            ViewBag.Destino = "FLOTA BARATO";
            return PartialView("~/Views/Home/Destinos/vuelosDestinos.cshtml", vuelos);
        }

        public IActionResult Equipaje() => View();
        public IActionResult Servicios_al_cliente()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                var consultas = _context.Consultas
                    .Where(c => c.idUsuario == userId.Value)
                    .OrderByDescending(c => c.idConsulta)
                    .ToList();
                ViewBag.Consultas = consultas;
            }
            return View();
        }
        public IActionResult Cargo() => View();
        public IActionResult Promociones() => View();
        public IActionResult Alianzas() => View();
        public IActionResult Flota()
        {
            var flota = _context.Flota.ToList();
            return View(flota);
        }
        public IActionResult Oficinas() => View();
        public IActionResult Autenticacion() => View("~/Views/Autenticacion/Autenticacion.cshtml");

        [HttpGet]
        [Route("Autenticacion/RecuperarContrasena")]
        public IActionResult RecuperarContrasena()
        {
            return View("~/Views/Autenticacion/RecuperarContrasena.cshtml");
        }

        [HttpPost]
        [Route("Autenticacion/RecuperarContrasena")]
        public async Task<IActionResult> RecuperarContrasena(string email, string dni, string nuevaContrasena, string confirmarContrasena)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(dni) || string.IsNullOrEmpty(nuevaContrasena) || string.IsNullOrEmpty(confirmarContrasena))
            {
                TempData["AuthError"] = "Por favor complete todos los campos";
                return View("~/Views/Autenticacion/RecuperarContrasena.cshtml");
            }

            if (nuevaContrasena != confirmarContrasena)
            {
                TempData["AuthError"] = "Las contraseñas no coinciden";
                return View("~/Views/Autenticacion/RecuperarContrasena.cshtml");
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.correoUsuario == email && u.identificacionUsuario == dni);

            if (usuario == null)
            {
                TempData["AuthError"] = "No se encontró una cuenta con esas credenciales";
                return View("~/Views/Autenticacion/RecuperarContrasena.cshtml");
            }

            // Actualizar la contraseña
            usuario.contrasenaUsuario = nuevaContrasena;
            await _context.SaveChangesAsync();

            TempData["AuthSuccess"] = "Contraseña actualizada correctamente";
            return RedirectToAction("Autenticacion");
        }

        public IActionResult panelAdministrador()
        {
            if (HttpContext.Session.GetString("UserType") != "admin")
            {
                return RedirectToAction("Index");
            }

            var usuarios = _context.Usuarios.ToList();
            var consultas = _context.Consultas
                .Include(c => c.Usuario)
                .OrderByDescending(c => c.urgencia == "Alto")
                .ThenByDescending(c => c.urgencia == "Medio")
                .ThenByDescending(c => c.urgencia == "Bajo")
                .ToList();

            ViewBag.Usuarios = usuarios;
            ViewBag.Consultas = consultas;

            return View("~/Views/Administrador/panelAdministrador.cshtml");
        }

        [HttpPost]
        public IActionResult CambiarRol([FromBody] CambiarRolModel model)
        {
            try
            {
                var usuario = _context.Usuarios.Find(model.id);
                if (usuario != null)
                {
                    usuario.esAdmin = model.esAdmin;
                    _context.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Usuario no encontrado" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult EliminarUsuario([FromBody] EliminarUsuarioModel model)
        {
            try
            {
                var usuario = _context.Usuarios.Find(model.id);
                if (usuario != null)
                {
                    _context.Usuarios.Remove(usuario);
                    _context.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Usuario no encontrado" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ObtenerUsuario(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return Json(new { success = false });
            }

            return Json(new
            {
                success = true,
                idUsuario = usuario.idUsuario,
                nombresUsuario = usuario.nombresUsuario,
                apellidosUsuario = usuario.apellidosUsuario,
                fechaNacimiento = usuario.nacimientoUsuario.ToString("yyyy-MM-dd"),
                telefonoUsuario = usuario.telefonoUsuario,
                identificacionUsuario = usuario.identificacionUsuario,
                correoUsuario = usuario.correoUsuario,
                discapacidad = usuario.discapacidad
            });
        }

        [HttpPost]
        public IActionResult ActualizarUsuario([FromBody] Usuario usuario)
        {
            try
            {
                if (usuario == null)
                {
                    return Json(new { success = false, message = "No se recibieron datos del usuario" });
                }

                var usuarioExistente = _context.Usuarios.Find(usuario.idUsuario);
                if (usuarioExistente == null)
                {
                    return Json(new { success = false, message = "No se encontró el usuario a actualizar" });
                }

                // Actualizar solo los campos que se envían y no son nulos
                if (!string.IsNullOrEmpty(usuario.nombresUsuario))
                    usuarioExistente.nombresUsuario = usuario.nombresUsuario;

                if (!string.IsNullOrEmpty(usuario.apellidosUsuario))
                    usuarioExistente.apellidosUsuario = usuario.apellidosUsuario;

                if (usuario.nacimientoUsuario != default(DateTime))
                {
                    usuarioExistente.nacimientoUsuario = usuario.nacimientoUsuario;
                }
                if (!string.IsNullOrEmpty(usuario.telefonoUsuario))
                {
                    usuarioExistente.telefonoUsuario = usuario.telefonoUsuario;
                }

                if (!string.IsNullOrEmpty(usuario.identificacionUsuario))
                    usuarioExistente.identificacionUsuario = usuario.identificacionUsuario;

                if (!string.IsNullOrEmpty(usuario.correoUsuario))
                    usuarioExistente.correoUsuario = usuario.correoUsuario;

                // La contraseña y el rol no se actualizan aquí
                // usuarioExistente.contrasenaUsuario = usuario.contrasenaUsuario;
                // usuarioExistente.esAdmin = usuario.esAdmin;

                _context.SaveChanges();
                return Json(new { success = true, message = "Usuario actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al actualizar: {ex.Message}" });
            }
        }

        [HttpPost]
        public IActionResult RegistrarConsulta(string motivoConsulta, string descripcionConsultas, string urgencia)
        {
            if (HttpContext.Session.GetString("UserType") == null)
            {
                return RedirectToAction("SelectLogin");
            }

            try
            {
                // Validar campos requeridos
                if (string.IsNullOrEmpty(motivoConsulta) || string.IsNullOrEmpty(urgencia))
                {
                    TempData["Error"] = "El motivo y la urgencia son campos requeridos";
                    return RedirectToAction("Servicios_al_cliente");
                }

                // Validar longitud de campos
                if (motivoConsulta.Length > 15)
                {
                    TempData["Error"] = "El motivo no puede tener más de 15 caracteres";
                    return RedirectToAction("Servicios_al_cliente");
                }

                if (descripcionConsultas != null && descripcionConsultas.Length > 200)
                {
                    TempData["Error"] = "La descripción no puede tener más de 200 caracteres";
                    return RedirectToAction("Servicios_al_cliente");
                }

                if (urgencia.Length > 10)
                {
                    TempData["Error"] = "La urgencia no puede tener más de 10 caracteres";
                    return RedirectToAction("Servicios_al_cliente");
                }

                // Validar valores permitidos para urgencia
                if (!new[] { "Alto", "Medio", "Bajo" }.Contains(urgencia))
                {
                    TempData["Error"] = "La urgencia debe ser Alto, Medio o Bajo";
                    return RedirectToAction("Servicios_al_cliente");
                }

                var userId = HttpContext.Session.GetInt32("UserId");
                if (!userId.HasValue)
                {
                    TempData["Error"] = "No se pudo identificar al usuario";
                    return RedirectToAction("Servicios_al_cliente");
                }

                // Verificar que el usuario existe
                var usuario = _context.Usuarios.Find(userId.Value);
                if (usuario == null)
                {
                    TempData["Error"] = "Usuario no encontrado";
                    return RedirectToAction("Servicios_al_cliente");
                }

                var consulta = new Consulta
                {
                    motivoConsulta = motivoConsulta,
                    descripcionConsultas = descripcionConsultas,
                    urgencia = urgencia,
                    estado = "Pendiente",
                    idUsuario = userId.Value
                };

                _context.Consultas.Add(consulta);
                _context.SaveChanges();

                TempData["Mensaje"] = "Consulta registrada exitosamente";
                return RedirectToAction("Servicios_al_cliente");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al registrar consulta: {ex.Message}");
                TempData["Error"] = "Error al registrar la consulta. Por favor, intente nuevamente.";
                return RedirectToAction("Servicios_al_cliente");
            }
        }

        [HttpPost]
        public IActionResult ActualizarEstadoConsulta([FromBody] ActualizarEstadoConsultaModel model)
        {
            try
            {
                var consulta = _context.Consultas.Find(model.id);
                if (consulta == null)
                {
                    return Json(new { success = false, message = "No se encontró la consulta" });
                }

                consulta.estado = model.estado;
                _context.SaveChanges();

                return Json(new { success = true, message = "Estado actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al actualizar estado: {ex.Message}" });
            }
        }

        [HttpPost]
        public IActionResult RegistrarPiloto([FromBody] Piloto piloto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                );
                return Json(new { success = false, errors = errors });
            }

            try
            {
                // Verificar si ya existe un piloto con el mismo DNI
                if (_context.Pilotos.Any(p => p.dniPiloto == piloto.dniPiloto))
                {
                    return Json(new { success = false, message = "Ya existe un piloto con este DNI" });
                }

                // Verificar si ya existe un piloto con la misma licencia
                if (_context.Pilotos.Any(p => p.licenciaPiloto == piloto.licenciaPiloto))
                {
                    return Json(new { success = false, message = "Ya existe un piloto con esta licencia" });
                }

                _context.Pilotos.Add(piloto);
                _context.SaveChanges();

                return Json(new { success = true, message = "Piloto registrado exitosamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al registrar piloto: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ObtenerPilotos()
        {
            try
            {
                Console.WriteLine("Obteniendo lista de pilotos...");
                var pilotos = _context.Pilotos.ToList();
                Console.WriteLine($"Pilotos encontrados: {pilotos.Count}");
                foreach (var piloto in pilotos)
                {
                    Console.WriteLine($"Piloto: {piloto.nombrePiloto} {piloto.apellidoPiloto} - ID: {piloto.idPiloto}");
                }
                return Json(new { success = true, data = pilotos });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener pilotos: {ex.Message}");
                return Json(new { success = false, message = "Error al obtener pilotos: " + ex.Message });
            }
        }

        [HttpPut]
        public IActionResult ActualizarPiloto(int id, [FromBody] Piloto piloto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                );
                return Json(new { success = false, errors = errors });
            }

            try
            {
                var pilotoExistente = _context.Pilotos.Find(id);
                if (pilotoExistente == null)
                {
                    return Json(new { success = false, message = "Piloto no encontrado" });
                }

                // Verificar si el DNI ya existe en otro piloto
                if (_context.Pilotos.Any(p => p.dniPiloto == piloto.dniPiloto && p.idPiloto != id))
                {
                    return Json(new { success = false, message = "Ya existe un piloto con este DNI" });
                }

                // Verificar si la licencia ya existe en otro piloto
                if (_context.Pilotos.Any(p => p.licenciaPiloto == piloto.licenciaPiloto && p.idPiloto != id))
                {
                    return Json(new { success = false, message = "Ya existe un piloto con esta licencia" });
                }

                // Actualizar los datos del piloto
                pilotoExistente.nombrePiloto = piloto.nombrePiloto;
                pilotoExistente.apellidoPiloto = piloto.apellidoPiloto;
                pilotoExistente.nacimientoPiloto = piloto.nacimientoPiloto;
                pilotoExistente.telefonoPiloto = piloto.telefonoPiloto;
                pilotoExistente.dniPiloto = piloto.dniPiloto;
                pilotoExistente.licenciaPiloto = piloto.licenciaPiloto;
                pilotoExistente.tipoLicPiloto = piloto.tipoLicPiloto;
                pilotoExistente.fechaEmiLic = piloto.fechaEmiLic;

                _context.SaveChanges();

                return Json(new { success = true, message = "Piloto actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al actualizar piloto: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult EliminarPiloto(int id)
        {
            try
            {
                var piloto = _context.Pilotos.Find(id);
                if (piloto == null)
                {
                    return Json(new { success = false, message = "Piloto no encontrado" });
                }

                _context.Pilotos.Remove(piloto);
                _context.SaveChanges();

                return Json(new { success = true, message = "Piloto eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar piloto: " + ex.Message });
            }
        }

        public IActionResult EditarPiloto(int id)
        {
            var piloto = _context.Pilotos.Find(id);
            if (piloto == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(piloto);
        }

        public IActionResult ObtenerPiloto(int id)
        {
            try
            {
                var piloto = _context.Pilotos.Find(id);
                if (piloto == null)
                {
                    return Json(new { success = false, message = "Piloto no encontrado" });
                }

                return Json(new { success = true, data = piloto });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al obtener datos del piloto: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ObtenerFlota()
        {
            try
            {
                var flota = _context.Flota.ToList();
                return Json(new { success = true, data = flota });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al obtener la flota: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult RegistrarAvion([FromBody] Flota avion)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = new Dictionary<string, string>();
                    foreach (var modelState in ModelState)
                    {
                        if (modelState.Value.Errors.Count > 0)
                        {
                            errors[modelState.Key] = modelState.Value.Errors[0].ErrorMessage;
                        }
                    }
                    return Json(new { success = false, errors = errors });
                }

                _context.Flota.Add(avion);
                _context.SaveChanges();
                return Json(new { success = true, message = "Avión registrado exitosamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al registrar avión: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ObtenerAvion(int id)
        {
            try
            {
                var avion = _context.Flota.Find(id);
                if (avion == null)
                {
                    return Json(new { success = false, message = "Avión no encontrado" });
                }
                return Json(new { success = true, data = avion });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al obtener avión: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult ActualizarAvion(int id, [FromBody] Flota avion)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                var avionExistente = _context.Flota.Find(id);
                if (avionExistente == null)
                {
                    return Json(new { success = false, message = "Avión no encontrado" });
                }

                avionExistente.modeloAvion = avion.modeloAvion;
                avionExistente.fabricanteAvion = avion.fabricanteAvion;
                avionExistente.matriculaAvion = avion.matriculaAvion;
                avionExistente.capacidadAvion = avion.capacidadAvion;
                avionExistente.claseAvion = avion.claseAvion;

                _context.SaveChanges();
                return Json(new { success = true, message = "Avión actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al actualizar avión: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult EliminarAvion(int id)
        {
            try
            {
                var avion = _context.Flota.Find(id);
                if (avion == null)
                {
                    return Json(new { success = false, message = "Avión no encontrado" });
                }

                _context.Flota.Remove(avion);
                _context.SaveChanges();
                return Json(new { success = true, message = "Avión eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar avión: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ObtenerVuelos()
        {
            try
            {
                var vuelos = _context.Vuelo
                    .Include(v => v.Avion)
                    .Include(v => v.Piloto)
                    .ToList();
                return Json(new { success = true, data = vuelos });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al obtener vuelos: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult RegistrarVuelo([FromBody] Vuelo vuelo)
        {
            try
            {
                if (vuelo == null)
                {
                    return Json(new { success = false, message = "Datos del vuelo no proporcionados" });
                }

                // Validar que el origen y destino sean diferentes
                if (vuelo.origenVuelo == vuelo.destinoVuelo)
                {
                    return Json(new { success = false, message = "El origen y destino no pueden ser iguales" });
                }

                // Validar que la fecha sea al menos 2 días en el futuro
                var fechaMinima = DateTime.Now.AddDays(2);
                if (vuelo.fechaVuelo < fechaMinima)
                {
                    return Json(new { success = false, message = "La fecha del vuelo debe ser al menos 2 días en el futuro" });
                }

                // Validar que el precio sea mayor a 0
                if (vuelo.precioVuelo <= 0)
                {
                    return Json(new { success = false, message = "El precio debe ser mayor a 0" });
                }

                _context.Vuelo.Add(vuelo);
                _context.SaveChanges();

                return Json(new { success = true, message = "Vuelo registrado exitosamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al registrar el vuelo: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ObtenerVuelo(int id)
        {
            try
            {
                var vuelo = _context.Vuelo
                    .Include(v => v.Avion)
                    .Include(v => v.Piloto)
                    .FirstOrDefault(v => v.idVuelo == id);
                
                if (vuelo == null)
                {
                    return Json(new { success = false, message = "Vuelo no encontrado" });
                }
                return Json(new { success = true, data = vuelo });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al obtener vuelo: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult ActualizarVuelo(int id, [FromBody] Vuelo vuelo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = new Dictionary<string, string>();
                    foreach (var modelState in ModelState)
                    {
                        if (modelState.Value.Errors.Count > 0)
                        {
                            errors[modelState.Key] = modelState.Value.Errors[0].ErrorMessage;
                        }
                    }
                    return Json(new { success = false, errors = errors });
                }

                // Validar que origen y destino no sean iguales
                if (vuelo.origenVuelo == vuelo.destinoVuelo)
                {
                    return Json(new { success = false, message = "El origen y destino no pueden ser la misma ciudad" });
                }

                var vueloExistente = _context.Vuelo.Find(id);
                if (vueloExistente == null)
                {
                    return Json(new { success = false, message = "Vuelo no encontrado" });
                }

                // Validar fecha (mínimo 2 días después de hoy)
                var fechaMinima = DateTime.Now.AddDays(2);
                if (vuelo.fechaVuelo < fechaMinima)
                {
                    return Json(new { success = false, message = "La fecha del vuelo debe ser al menos 2 días después de hoy" });
                }

                vueloExistente.origenVuelo = vuelo.origenVuelo;
                vueloExistente.destinoVuelo = vuelo.destinoVuelo;
                vueloExistente.fechaVuelo = vuelo.fechaVuelo;
                vueloExistente.idAvion = vuelo.idAvion;
                vueloExistente.idPiloto = vuelo.idPiloto;
                vueloExistente.precioVuelo = vuelo.precioVuelo;

                _context.SaveChanges();
                return Json(new { success = true, message = "Vuelo actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al actualizar vuelo: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult EliminarVuelo(int id)
        {
            try
            {
                var vuelo = _context.Vuelo.Find(id);
                if (vuelo == null)
                {
                    return Json(new { success = false, message = "Vuelo no encontrado" });
                }

                _context.Vuelo.Remove(vuelo);
                _context.SaveChanges();
                return Json(new { success = true, message = "Vuelo eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar vuelo: " + ex.Message });
            }
        }

        public class CambiarRolModel
        {
            public int id { get; set; }
            public bool esAdmin { get; set; }
        }

        public class EliminarUsuarioModel
        {
            public int id { get; set; }
        }

        public class ActualizarEstadoConsultaModel
        {
            public int id { get; set; }
            public string estado { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> ReservarVuelo(int idVuelo, int numPasajeros, string mascotas)
        {
            try
            {
                // Validar que el usuario esté autenticado
                var userId = HttpContext.Session.GetInt32("UserId");
                if (!userId.HasValue)
                {
                    TempData["Error"] = "Debe iniciar sesión para realizar una reserva";
                    return RedirectToAction("Login", "Home");
                }

                // Validar que el usuario no sea administrador
                var usuario = await _context.Usuarios.FindAsync(userId.Value);
                if (usuario == null)
                {
                    TempData["Error"] = "Usuario no encontrado";
                    return RedirectToAction("Login", "Home");
                }

                if (usuario.esAdmin)
                {
                    TempData["Error"] = "Los administradores no pueden realizar reservas";
                    return RedirectToAction("Index", "Home");
                }

                // Validar el número máximo de reservas (3)
                var totalReservasUsuario = await _context.ReservaVuelos
                    .Where(r => r.idUsuario == userId.Value)
                    .CountAsync();

                if (totalReservasUsuario >= 3)
                {
                    TempData["Error"] = "Ha alcanzado el límite máximo de 3 reservas";
                    return RedirectToAction("Index", "Home");
                }

                // Validar el vuelo
                var vuelo = await _context.Vuelo.FindAsync(idVuelo);
                if (vuelo == null)
                {
                    TempData["Error"] = "El vuelo seleccionado no existe";
                    return RedirectToAction("Index", "Home");
                }

                // Validar que el vuelo no esté en el pasado
                if (vuelo.fechaVuelo < DateTime.Now)
                {
                    TempData["Error"] = "No se pueden reservar vuelos en el pasado";
                    return RedirectToAction("Index", "Home");
                }

                // Validar número de pasajeros
                if (numPasajeros < 1 || numPasajeros > 8)
                {
                    TempData["Error"] = "El número de pasajeros debe estar entre 1 y 8";
                    return RedirectToAction("Index", "Home");
                }

                // Validar que haya suficientes asientos disponibles
                var reservasExistentes = await _context.ReservaVuelos
                    .Where(r => r.idVuelo == idVuelo)
                    .SumAsync(r => r.personasResVue);

                var avion = await _context.Flota.FindAsync(vuelo.idAvion);
                if (avion == null)
                {
                    TempData["Error"] = "Error al verificar la capacidad del avión";
                    return RedirectToAction("Index", "Home");
                }

                if (reservasExistentes + numPasajeros > avion.capacidadAvion)
                {
                    TempData["Error"] = "No hay suficientes asientos disponibles para este vuelo";
                    return RedirectToAction("Index", "Home");
                }

                // Validar datos de pasajeros adicionales
                if (numPasajeros > 1)
                {
                    for (int i = 1; i < numPasajeros; i++)
                    {
                        var nombre = Request.Form[$"nombre{i}"].ToString();
                        var dni = Request.Form[$"dni{i}"].ToString();
                        var edadStr = Request.Form[$"edad{i}"].ToString();

                        if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(dni) || string.IsNullOrEmpty(edadStr))
                        {
                            TempData["Error"] = "Debe completar todos los datos de los pasajeros adicionales";
                            return RedirectToAction("Index", "Home");
                        }

                        if (!int.TryParse(edadStr, out int edad) || edad < 0 || edad > 120)
                        {
                            TempData["Error"] = "La edad de los pasajeros debe ser válida";
                            return RedirectToAction("Index", "Home");
                        }

                        if (dni.Length != 8 || !dni.All(char.IsDigit))
                        {
                            TempData["Error"] = "El DNI debe tener 8 dígitos numéricos";
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }

                var reserva = new ReservaVuelo
                {
                    idVuelo = idVuelo,
                    origenResVue = vuelo.origenVuelo,
                    destinoVuelo = vuelo.destinoVuelo,
                    personasResVue = numPasajeros,
                    mascotasResVue = mascotas,
                    precioVuelo = vuelo.precioVuelo * numPasajeros + (mascotas.ToLower() == "si" ? 50 : 0),
                    pagadoVuelo = false,
                    idUsuario = userId.Value
                };

                _context.ReservaVuelos.Add(reserva);
                await _context.SaveChangesAsync();

                // Guardar el pasajero principal (usuario logueado)
                var pasajeroPrincipal = new Pasajero
                {
                    Nombre = $"{usuario.nombresUsuario} {usuario.apellidosUsuario}",
                    DNI = usuario.identificacionUsuario,
                    Edad = DateTime.Now.Year - usuario.nacimientoUsuario.Year,
                    EsMenorEdad = DateTime.Now.Year - usuario.nacimientoUsuario.Year < 18,
                    ReservaVueloId = reserva.idResVuelo
                };
                _context.Pasajeros.Add(pasajeroPrincipal);

                // Guardar los pasajeros adicionales
                for (int i = 1; i < numPasajeros; i++)
                {
                    var nombre = Request.Form[$"nombre{i}"].ToString();
                    var dni = Request.Form[$"dni{i}"].ToString();
                    var edad = int.Parse(Request.Form[$"edad{i}"].ToString());
                    var esMenorEdad = edad < 18;

                    var pasajero = new Pasajero
                    {
                        Nombre = nombre,
                        DNI = dni,
                        Edad = edad,
                        EsMenorEdad = esMenorEdad,
                        ReservaVueloId = reserva.idResVuelo
                    };
                    _context.Pasajeros.Add(pasajero);
                }
                await _context.SaveChangesAsync();

                TempData["Success"] = "Reserva realizada con éxito";
                return RedirectToAction("DetalleReserva", new { id = reserva.idResVuelo });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear la reserva: {ex.Message}");
                TempData["Error"] = "Error al crear la reserva. Por favor, intente nuevamente.";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> DetalleReserva(int id)
        {
            var reserva = await _context.ReservaVuelos
                .Include(r => r.Vuelo)
                .Include(r => r.Pasajeros)
                .FirstOrDefaultAsync(r => r.idResVuelo == id);

            if (reserva == null)
            {
                TempData["Error"] = "Reserva no encontrada";
                return RedirectToAction("Index", "Home");
            }

            return View("~/Views/Home/Destinos/DetalleReserva.cshtml", reserva);
        }

        [HttpGet]
        public async Task<IActionResult> ReservarVuelo(int idVuelo)
        {
            var vuelo = await _context.Vuelo.FindAsync(idVuelo);
            if (vuelo == null)
            {
                return NotFound();
            }
            return View("~/Views/Home/Destinos/ReservarVuelo.cshtml", vuelo);
        }

        public IActionResult Reservas()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return View(new List<ReservaVuelo>());
            }

            var reservas = _context.ReservaVuelos
                .Where(r => r.idUsuario == userId.Value)
                .ToList();

            return View(reservas);
        }

        [HttpGet]
        public IActionResult ObtenerDetallesVuelo(int id)
        {
            try
            {
                var reservas = _context.ReservaVuelos
                    .Include(r => r.Usuario)
                    .Include(r => r.Vuelo)
                    .Where(r => r.Vuelo.idVuelo == id)
                    .OrderByDescending(r => r.Vuelo.fechaVuelo)
                    .ToList();

                return Json(new { success = true, data = reservas });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al obtener detalles del vuelo: " + ex.Message });
            }
        }

        public IActionResult perfilPasajero()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login");
            }

            var usuario = _context.Usuarios.Find(userId.Value);
            if (usuario == null)
            {
                return RedirectToAction("Login");
            }

            return View(usuario);
        }

        [HttpPost]
        public IActionResult ActualizarPerfil(Usuario usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("perfilPasajero", usuario);
                }

                var usuarioExistente = _context.Usuarios.Find(usuario.idUsuario);
                if (usuarioExistente == null)
                {
                    TempData["Error"] = "Usuario no encontrado";
                    return RedirectToAction("perfilPasajero");
                }

                // Actualizar los datos del usuario
                usuarioExistente.nombresUsuario = usuario.nombresUsuario;
                usuarioExistente.apellidosUsuario = usuario.apellidosUsuario;
                usuarioExistente.nacimientoUsuario = usuario.nacimientoUsuario;
                usuarioExistente.telefonoUsuario = usuario.telefonoUsuario;
                usuarioExistente.correoUsuario = usuario.correoUsuario;
                usuarioExistente.contrasenaUsuario = usuario.contrasenaUsuario;
                usuarioExistente.discapacidad = usuario.discapacidad;

                _context.SaveChanges();

                // Actualizar la sesión
                HttpContext.Session.SetString("UserName", usuario.nombresUsuario);

                TempData["Success"] = "Perfil actualizado correctamente";
                return RedirectToAction("perfilPasajero");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al actualizar el perfil: " + ex.Message;
                return View("perfilPasajero", usuario);
            }
        }
    }
}
