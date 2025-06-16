using System;
using System.ComponentModel.DataAnnotations;

namespace AeroLinea.Models
{
    public class Pago
    {
        [Key]
        public int IdPago { get; set; }
        public int IdReserva { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }
        public string SessionId { get; set; }
        public DateTime FechaPago { get; set; }
        public string MetodoPago { get; set; }
        public string EmailCliente { get; set; }
        public string NombreCliente { get; set; }
        public string Descripcion { get; set; }

        // Relaciones
        public ReservaVuelo Reserva { get; set; }
    }
} 