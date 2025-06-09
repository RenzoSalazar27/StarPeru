using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AeroLinea.Models
{
    public class Piloto
    {
        [Key]
        public int idPiloto { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(30, ErrorMessage = "El nombre no puede tener más de 30 caracteres")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]{5,30}$", ErrorMessage = "El nombre solo debe contener letras y espacios, mínimo 5 caracteres")]
        public string nombrePiloto { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(30, ErrorMessage = "El apellido no puede tener más de 30 caracteres")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]{5,30}$", ErrorMessage = "El apellido solo debe contener letras y espacios, mínimo 5 caracteres")]
        public string apellidoPiloto { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [CustomValidation(typeof(Piloto), "ValidarFechaNacimiento")]
        public DateTime nacimientoPiloto { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [RegularExpression(@"^[0-9]{9}$", ErrorMessage = "El teléfono debe tener 9 dígitos")]
        public int telefonoPiloto { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 dígitos")]
        [RegularExpression(@"^[0-9]{8}$", ErrorMessage = "El DNI solo debe contener números")]
        public string DNI { get; set; }

        [Required(ErrorMessage = "La licencia es obligatoria")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "La licencia debe tener 9 dígitos")]
        [RegularExpression(@"^[0-9]{9}$", ErrorMessage = "La licencia solo debe contener números")]
        public string licenciaPiloto { get; set; }

        [Required(ErrorMessage = "El tipo de licencia es obligatorio")]
        [StringLength(20)]
        public string tipoLicPiloto { get; set; }

        [Required(ErrorMessage = "La fecha de emisión de la licencia es obligatoria")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [CustomValidation(typeof(Piloto), "ValidarFechaEmisionLicencia")]
        public DateTime fechaEmiLic { get; set; }

        // Método de validación personalizado para la fecha de nacimiento
        public static ValidationResult ValidarFechaNacimiento(DateTime fecha, ValidationContext context)
        {
            if (fecha == DateTime.MinValue)
                return new ValidationResult("La fecha de nacimiento no puede ser 0000-00-00");

            if (fecha > DateTime.Now)
                return new ValidationResult("La fecha de nacimiento no puede ser futura");

            return ValidationResult.Success;
        }

        // Método de validación personalizado para la fecha de emisión de licencia
        public static ValidationResult ValidarFechaEmisionLicencia(DateTime fecha, ValidationContext context)
        {
            if (fecha == DateTime.MinValue)
                return new ValidationResult("La fecha de emisión no puede ser 0000-00-00");

            if (fecha > DateTime.Now)
                return new ValidationResult("La fecha de emisión no puede ser futura");

            return ValidationResult.Success;
        }
    }
} 