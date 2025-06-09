using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AeroLinea.Models
{
    public class Usuario
    {
        public int idUsuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "El nombre debe tener entre 5 y 30 caracteres")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "El nombre solo debe contener letras y espacios")]
        public string nombresUsuario { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "El apellido debe tener entre 5 y 30 caracteres")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "El apellido solo debe contener letras y espacios")]
        public string apellidosUsuario { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [CustomValidation(typeof(Usuario), "ValidarFechaNacimiento")]
        public DateTime nacimientoUsuario { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [RegularExpression(@"^[0-9]{9}$", ErrorMessage = "El teléfono debe tener 9 dígitos")]
        public int telefonoUsuario { get; set; }

        [Required(ErrorMessage = "La identificación es obligatoria")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "La identificación debe tener 8 caracteres")]
        [CustomValidation(typeof(Usuario), "ValidarIdentificacion")]
        public string identificacionUsuario { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        [StringLength(30, ErrorMessage = "El correo no puede exceder los 30 caracteres")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "El formato del correo no es válido")]
        public string correoUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 20 caracteres")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{6,20}$", 
            ErrorMessage = "La contraseña debe contener letras, números y caracteres especiales")]
        public string contrasenaUsuario { get; set; }

        [Required(ErrorMessage = "Debe indicar si tiene discapacidad")]
        public bool discapacidad { get; set; }

        [Required]
        public bool esAdmin { get; set; }

        public static ValidationResult ValidarFechaNacimiento(DateTime fecha, ValidationContext context)
        {
            if (fecha == DateTime.MinValue)
                return new ValidationResult("La fecha no puede ser 0000-00-00");

            if (fecha > DateTime.Now)
                return new ValidationResult("La fecha no puede ser posterior a la fecha actual");

            return ValidationResult.Success;
        }

        public static ValidationResult ValidarIdentificacion(string identificacion, ValidationContext context)
        {
            // Validar DNI (8 números)
            if (Regex.IsMatch(identificacion, @"^\d{8}$"))
                return ValidationResult.Success;

            // Validar Pasaporte (8 caracteres alfanuméricos)
            if (Regex.IsMatch(identificacion, @"^[a-zA-Z0-9]{8}$"))
                return ValidationResult.Success;

            return new ValidationResult("La identificación debe ser un DNI de 8 números o un Pasaporte de 8 caracteres alfanuméricos");
        }
    }
} 