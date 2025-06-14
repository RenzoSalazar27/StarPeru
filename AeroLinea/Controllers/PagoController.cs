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
                    ViewBag.Reserva = await _context.ReservaVuelos
                        .Include(r => r.Pasajeros)
                        .FirstOrDefaultAsync(r => r.idResVuelo == pago.idReserva);
                    return View("ProcesoPago", pago);
                }

                // Obtener la reserva y el vuelo
                var reserva = await _context.ReservaVuelos
                    .Include(r => r.Vuelo)
                    .ThenInclude(v => v.Avion)
                    .FirstOrDefaultAsync(r => r.idResVuelo == pago.idReserva);

                if (reserva == null)
                {
                    ModelState.AddModelError("", "Reserva no encontrada");
                    ViewBag.Reserva = await _context.ReservaVuelos
                        .Include(r => r.Pasajeros)
                        .FirstOrDefaultAsync(r => r.idResVuelo == pago.idReserva);
                    return View("ProcesoPago", pago);
                }

                // Verificar que la reserva no esté pagada
                if (reserva.pagadoVuelo)
                {
                    ModelState.AddModelError("", "Esta reserva ya ha sido pagada");
                    ViewBag.Reserva = await _context.ReservaVuelos
                        .Include(r => r.Pasajeros)
                        .FirstOrDefaultAsync(r => r.idResVuelo == pago.idReserva);
                    return View("ProcesoPago", pago);
                }

                // Verificar que el vuelo no esté en el pasado
                if (reserva.Vuelo.fechaVuelo < DateTime.Now)
                {
                    ModelState.AddModelError("", "No se puede pagar un vuelo en el pasado");
                    ViewBag.Reserva = await _context.ReservaVuelos
                        .Include(r => r.Pasajeros)
                        .FirstOrDefaultAsync(r => r.idResVuelo == pago.idReserva);
                    return View("ProcesoPago", pago);
                }

                // Verificar que haya suficientes asientos disponibles
                var reservasExistentes = await _context.ReservaVuelos
                    .Where(r => r.idVuelo == reserva.idVuelo && r.pagadoVuelo)
                    .SumAsync(r => r.personasResVue);

                if (reservasExistentes + reserva.personasResVue > reserva.Vuelo.Avion.capacidadAvion)
                {
                    ModelState.AddModelError("", "No hay suficientes asientos disponibles para este vuelo");
                    ViewBag.Reserva = await _context.ReservaVuelos
                        .Include(r => r.Pasajeros)
                        .FirstOrDefaultAsync(r => r.idResVuelo == pago.idReserva);
                    return View("ProcesoPago", pago);
                }

                // Establecer la fecha del pago
                pago.fechaPago = DateTime.Now;

                // Lógica para cuotas e intereses
                if (pago.metodoPago == "Tarjeta de Credito")
                {
                    // precioTotal ya tiene el valor original
                    // precioFinal ya viene con el 5% de interés desde el frontend
                    if (pago.Cuotas.HasValue && pago.Cuotas.Value > 0)
                    {
                        pago.PrecioPorCuota = pago.precioFinal / pago.Cuotas.Value;
                    }
                    else
                    {
                        // Fallback: si no se envía cuotas o es 0, se asume 1 cuota
                        pago.Cuotas = 1;
                        pago.PrecioPorCuota = pago.precioFinal;
                    }
                }
                else // Tarjeta de Debito
                {
                    pago.Cuotas = null;
                    pago.PrecioPorCuota = null;
                    // Para tarjeta de débito, el precio final es el precio total sin intereses
                    pago.precioFinal = pago.precioTotal;
                }

                // Guardar el pago
                _context.Pagos.Add(pago);

                // Actualizar el estado de la reserva
                reserva.pagadoVuelo = true;

                await _context.SaveChangesAsync();

                return RedirectToAction("ConfirmacionPago", new { id = reserva.idResVuelo });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al procesar el pago: " + ex.Message);
                ViewBag.Reserva = await _context.ReservaVuelos
                    .Include(r => r.Pasajeros)
                    .FirstOrDefaultAsync(r => r.idResVuelo == pago.idReserva);
                return View("ProcesoPago", pago);
            }
        }

        public IActionResult ConfirmacionPago(int id)
        {
            ViewBag.IdReserva = id;
            return View();
        }
    }
} 