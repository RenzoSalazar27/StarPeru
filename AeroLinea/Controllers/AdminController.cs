using Microsoft.AspNetCore.Mvc;
using AeroLinea.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using AeroLinea.Filters;

namespace AeroLinea.Controllers
{
    public class AdminController : Controller
    {
        // Credenciales hardcodeadas (en un caso real deberían estar en una base de datos segura)
        private const string AdminUsername = "admin";
        private const string AdminPassword = "123456";

        public IActionResult Login()
        {
            // Si ya hay una sesión de usuario normal, cerrarla
            if (HttpContext.Session.GetString("UserEmail") != null)
            {
                HttpContext.Session.Remove("UserEmail");
            }

            // Si ya hay una sesión de administrador, redirigir al panel
            if (HttpContext.Session.GetString("AdminUsername") != null)
            {
                return RedirectToAction("panelAdministrador", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(AdminLogin model)
        {
            if (ModelState.IsValid)
            {
                if (model.Username == AdminUsername && model.Password == AdminPassword)
                {
                    // Cerrar cualquier sesión de usuario si existe
                    if (HttpContext.Session.GetString("UserEmail") != null)
                    {
                        HttpContext.Session.Remove("UserEmail");
                    }
                    
                    // Guardar la sesión del administrador
                    HttpContext.Session.SetString("AdminUsername", model.Username);
                    return RedirectToAction("panelAdministrador", "Home");
                }
                ModelState.AddModelError("", "Usuario o contraseña incorrectos");
            }
            return View(model);
        }

        [TypeFilter(typeof(AdminAuthFilter))]
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminUsername");
            return RedirectToAction("Index", "Home");
        }
    }
} 