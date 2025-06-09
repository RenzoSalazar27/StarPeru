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
                    return RedirectToAction("Index", "Home");
                }
                TempData["Error"] = "El correo o contraseña no son correctos";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al iniciar sesión: " + ex.Message;
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
        public IActionResult Destinos() => View();
        public IActionResult Equipaje() => View();
        public IActionResult Servicios_al_cliente() => View();
        public IActionResult Cargo() => View();
        public IActionResult Promociones() => View();
        public IActionResult Alianzas() => View();
        public IActionResult Flota() => View();
        public IActionResult Oficinas() => View();
        public IActionResult Autenticacion() => View("~/Views/Autenticacion/Autenticacion.cshtml");
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
                TempData["Error"] = "Error al registrar la consulta: " + ex.Message;
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
                if (_context.Pilotos.Any(p => p.DNI == piloto.DNI))
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
                var pilotos = _context.Pilotos.ToList();
                return Json(new { success = true, data = pilotos });
            }
            catch (Exception ex)
            {
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
                if (_context.Pilotos.Any(p => p.DNI == piloto.DNI && p.idPiloto != id))
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
                pilotoExistente.DNI = piloto.DNI;
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
    }
}
