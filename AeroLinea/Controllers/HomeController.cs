using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AeroLinea.Models;
using System.Diagnostics;

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
            var usuario = _context.Usuarios.FirstOrDefault(u => u.correoUsuario == email && u.contrasenaUsuario == password);
            if (usuario != null)
            {
                HttpContext.Session.SetString("UserEmail", usuario.correoUsuario);
                HttpContext.Session.SetString("UserName", usuario.nombresUsuario);
                HttpContext.Session.SetString("UserType", usuario.esAdmin ? "admin" : "user");
                return RedirectToAction("Index", "Home");
            }
            TempData["Error"] = "El correo o contraseña no son correctos";
            return View("~/Views/Autenticacion/Autenticacion.cshtml");
        }

        public IActionResult Registro()
        {
            return View("~/Views/Autenticacion/registroPasajero.cshtml");
        }

        [HttpPost]
        public IActionResult registroPasajero(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.esAdmin = false;
                var usuarioExistente = _context.Usuarios.FirstOrDefault(u => u.correoUsuario == usuario.correoUsuario);
                if (usuarioExistente != null)
                {
                    ModelState.AddModelError("correoUsuario", "El correo electrónico ya está registrado");
                    return View(usuario);
                }

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                HttpContext.Session.SetString("UserEmail", usuario.correoUsuario);
                HttpContext.Session.SetString("UserName", usuario.nombresUsuario);
                HttpContext.Session.SetString("UserType", "user");

                return RedirectToAction("Index", "Home");
            }
            return View(usuario);
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
                return RedirectToAction("Index", "Home");
            }
            var usuarios = _context.Usuarios.ToList();
            ViewBag.Usuarios = usuarios;
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
                    usuarioExistente.nacimientoUsuario = usuario.nacimientoUsuario;

                if (usuario.telefonoUsuario != 0)
                    usuarioExistente.telefonoUsuario = usuario.telefonoUsuario;

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

        public class CambiarRolModel
        {
            public int id { get; set; }
            public bool esAdmin { get; set; }
        }

        public class EliminarUsuarioModel
        {
            public int id { get; set; }
        }
    }
}
