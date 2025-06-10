using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AeroLinea.Models;
using Data;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace AeroLinea.Controllers
{
    public class PagoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ProcesoPago(int id)
        {
            var reserva = await _context.ReservaVuelos
                .Include(r => r.Pasajeros)
                .FirstOrDefaultAsync(r => r.idResVuelo == id);

            if (reserva == null)
            {
                return RedirectToAction("Error", "Home", new { mensaje = "Reserva no encontrada" });
            }

            ViewBag.Reserva = reserva;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcesarPago(Pago pago)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errores = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    return Json(new { 
                        success = false, 
                        message = "Datos de pago inválidos",
                        errors = errores
                    });
                }

                // Obtener la reserva y el vuelo
                var reserva = await _context.ReservaVuelos
                    .Include(r => r.Vuelo)
                    .ThenInclude(v => v.Avion)
                    .FirstOrDefaultAsync(r => r.idResVuelo == pago.idReserva);

                if (reserva == null)
                {
                    return Json(new { success = false, message = "Reserva no encontrada" });
                }

                // Verificar que la reserva no esté pagada
                if (reserva.pagadoVuelo)
                {
                    return Json(new { success = false, message = "Esta reserva ya ha sido pagada" });
                }

                // Verificar que el vuelo no esté en el pasado
                if (reserva.Vuelo.fechaVuelo < DateTime.Now)
                {
                    return Json(new { success = false, message = "No se puede pagar un vuelo en el pasado" });
                }

                // Verificar que haya suficientes asientos disponibles
                var reservasExistentes = await _context.ReservaVuelos
                    .Where(r => r.idVuelo == reserva.idVuelo && r.pagadoVuelo)
                    .SumAsync(r => r.personasResVue);

                if (reservasExistentes + reserva.personasResVue > reserva.Vuelo.Avion.capacidadAvion)
                {
                    return Json(new { success = false, message = "No hay suficientes asientos disponibles para este vuelo" });
                }

                // Establecer la fecha del pago
                pago.fechaPago = DateTime.Now;

                // Guardar el pago
                _context.Pagos.Add(pago);

                // Actualizar el estado de la reserva
                reserva.pagadoVuelo = true;

                await _context.SaveChangesAsync();

                return Json(new { 
                    success = true, 
                    message = "Pago procesado exitosamente",
                    redirectUrl = Url.Action("ConfirmacionPago", "Pago", new { id = reserva.idResVuelo })
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al procesar el pago: " + ex.Message });
            }
        }

        public IActionResult ConfirmacionPago(int id)
        {
            ViewBag.IdReserva = id;
            return View();
        }
    }
} 