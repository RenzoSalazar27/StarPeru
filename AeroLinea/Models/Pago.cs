using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AeroLinea.Models
{
    public class Pago
    {
        [Key]
        public int idPago { get; set; }

        [Required(ErrorMessage = "El ID de la reserva es obligatorio")]
        [ForeignKey("ReservaVuelo")]
        public int idReserva { get; set; }

        [Required(ErrorMessage = "El método de pago es obligatorio")]
        [StringLength(20)]
        public string metodoPago { get; set; }

        [Required(ErrorMessage = "El nombre del titular es obligatorio")]
        [StringLength(100)]
        public string nombreTitularPago { get; set; }

        [Required(ErrorMessage = "El correo para factura es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        [StringLength(100)]
        public string correoFacturaPago { get; set; }

        [Required(ErrorMessage = "El banco/tarjeta es obligatorio")]
        [StringLength(20)]
        public string bancoTarjetaPago { get; set; }

        [Required(ErrorMessage = "El número de tarjeta es obligatorio")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "El número de tarjeta debe tener 16 dígitos")]
        [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "El número de tarjeta solo debe contener números")]
        public string numeroTarjetaPago { get; set; }

        [Required(ErrorMessage = "La fecha de expiración es obligatoria")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "La fecha de expiración debe tener el formato MM/AA")]
        [RegularExpression(@"^(0[1-9]|1[0-2])/([0-9]{2})$", ErrorMessage = "La fecha de expiración debe tener el formato MM/AA")]
        public string fechaExpiracionPago { get; set; }

        [Required(ErrorMessage = "El CVV/CVC es obligatorio")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "El CVV/CVC debe tener 3 dígitos")]
        [RegularExpression(@"^[0-9]{3}$", ErrorMessage = "El CVV/CVC solo debe contener números")]
        public string cvvCvcPago { get; set; }

        [Required(ErrorMessage = "El número de celular es obligatorio")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "El número de celular debe tener 9 dígitos")]
        [RegularExpression(@"^[0-9]{9}$", ErrorMessage = "El número de celular solo debe contener números")]
        public string numCelularPago { get; set; }

        [Required(ErrorMessage = "El precio total es obligatorio")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal precioTotal { get; set; }

        [Required(ErrorMessage = "El precio final es obligatorio")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal precioFinal { get; set; }

        [Required(ErrorMessage = "La aceptación de términos es obligatoria")]
        public bool aceptoTerminos { get; set; }

        [Required]
        public DateTime fechaPago { get; set; }

        // Nuevas propiedades para pagos con tarjeta de crédito
        public int? Cuotas { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? PrecioPorCuota { get; set; }

        public virtual ReservaVuelo? ReservaVuelo { get; set; }
    }
} 