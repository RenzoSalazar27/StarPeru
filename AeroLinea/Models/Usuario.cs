using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace AeroLinea.Models
{
    public class Usuario
    {
        [Key]
        public int idUsuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "El nombre debe tener entre 5 y 30 caracteres")]
        public string nombresUsuario { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "El apellido debe tener entre 5 y 30 caracteres")]
        public string apellidosUsuario { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime nacimientoUsuario { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "El teléfono debe tener 9 dígitos")]
        public string telefonoUsuario { get; set; }

        [Required(ErrorMessage = "La identificación es obligatoria")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "La identificación debe tener 8 caracteres")]
        public string identificacionUsuario { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        [StringLength(30, ErrorMessage = "El correo no puede exceder los 30 caracteres")]
        public string correoUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 20 caracteres")]
        public string contrasenaUsuario { get; set; }

        [Required(ErrorMessage = "Debe indicar si tiene discapacidad")]
        public bool discapacidad { get; set; }

        [Required(ErrorMessage = "Debe indicar la condición del usuario")]
        [StringLength(50, ErrorMessage = "La condición no puede exceder los 50 caracteres")]
        public string? condicionUsuario { get; set; }

        [Required]
        public bool esAdmin { get; set; }

        // Relación con Consultas (opcional)
        public virtual ICollection<Consulta>? Consultas { get; set; }
    }
} 