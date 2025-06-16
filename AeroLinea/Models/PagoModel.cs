using System;

namespace AeroLinea.Models
{
    public class PagoModel
    {
        public int IdReserva { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; }
        public string EmailCliente { get; set; }
        public string NombreCliente { get; set; }
        public DateTime FechaPago { get; set; }
    }
} 