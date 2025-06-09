using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AeroLinea.Models
{
    public class Vuelo
    {
        [Key]
        public int idVuelo { get; set; }

        [Required(ErrorMessage = "El origen es obligatorio")]
        [StringLength(50, ErrorMessage = "El origen no puede tener más de 50 caracteres")]
        [RegularExpression(@"^(Chiclayo|Cajamarca|Huanuco|Lima|Iquitos|Tarapoto|Pucallpa)$", 
            ErrorMessage = "El origen debe ser una de las siguientes ciudades: Chiclayo, Cajamarca, Huanuco, Lima, Iquitos, Tarapoto, Pucallpa")]
        public string origenVuelo { get; set; }

        [Required(ErrorMessage = "El destino es obligatorio")]
        [StringLength(50, ErrorMessage = "El destino no puede tener más de 50 caracteres")]
        [RegularExpression(@"^(Chiclayo|Cajamarca|Huanuco|Lima|Iquitos|Tarapoto|Pucallpa)$", 
            ErrorMessage = "El destino debe ser una de las siguientes ciudades: Chiclayo, Cajamarca, Huanuco, Lima, Iquitos, Tarapoto, Pucallpa")]
        public string destinoVuelo { get; set; }

        [Required(ErrorMessage = "La fecha del vuelo es obligatoria")]
        public DateTime fechaVuelo { get; set; }

        [Required(ErrorMessage = "El avión es obligatorio")]
        public int idAvion { get; set; }

        [Required(ErrorMessage = "El piloto es obligatorio")]
        public int idPiloto { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, 999999.99, ErrorMessage = "El precio debe estar entre 0.01 y 999999.99")]
        [Column(TypeName = "decimal(6,2)")]
        public decimal precioVuelo { get; set; }

        [ForeignKey("idAvion")]
        public virtual Flota Avion { get; set; }

        [ForeignKey("idPiloto")]
        public virtual Piloto Piloto { get; set; }
    }
} 