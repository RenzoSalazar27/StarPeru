using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AeroLinea.Models
{
    public class Piloto
    {
        [Key]
        public int idPiloto { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(30, ErrorMessage = "El nombre no puede tener más de 30 caracteres")]
        public string nombrePiloto { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(30, ErrorMessage = "El apellido no puede tener más de 30 caracteres")]
        public string apellidoPiloto { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime nacimientoPiloto { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        public int telefonoPiloto { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 dígitos")]
        public string dniPiloto { get; set; }

        [Required(ErrorMessage = "La licencia es obligatoria")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "La licencia debe tener 9 dígitos")]
        public string licenciaPiloto { get; set; }

        [Required(ErrorMessage = "El tipo de licencia es obligatorio")]
        [StringLength(20)]
        public string tipoLicPiloto { get; set; }

        [Required(ErrorMessage = "La fecha de emisión de la licencia es obligatoria")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fechaEmiLic { get; set; }
    }
} 