using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AeroLinea.Models
{
    public class Pasajero
    {
        [Key]
        public int idPasajero { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [StringLength(8, ErrorMessage = "El DNI debe tener 8 caracteres")]
        public string DNI { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La edad es obligatoria")]
        [Range(0, 120, ErrorMessage = "La edad debe estar entre 0 y 120 años")]
        public int Edad { get; set; }

        [Required(ErrorMessage = "El estado de menor de edad es obligatorio")]
        public bool EsMenorEdad { get; set; }

        [Required(ErrorMessage = "El ID de la reserva es obligatorio")]
        public int ReservaVueloId { get; set; }

        [ForeignKey("ReservaVueloId")]
        public virtual ReservaVuelo ReservaVuelo { get; set; }
    }
} 