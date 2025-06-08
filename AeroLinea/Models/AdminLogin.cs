using System.ComponentModel.DataAnnotations;

namespace AeroLinea.Models
{
    public class AdminLogin
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
} 