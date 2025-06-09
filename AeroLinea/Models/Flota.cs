using System;
using System.ComponentModel.DataAnnotations;

namespace AeroLinea.Models
{
    public class Flota
    {
        [Key]
        public int idAvion { get; set; }

        [Required(ErrorMessage = "El modelo del avión es requerido")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "El modelo debe tener entre 5 y 15 caracteres")]
        public string modeloAvion { get; set; }

        [Required(ErrorMessage = "El fabricante es requerido")]
        [StringLength(10, ErrorMessage = "El fabricante no puede exceder los 10 caracteres")]
        public string fabricanteAvion { get; set; }

        [Required(ErrorMessage = "La matrícula es requerida")]
        [StringLength(8, MinimumLength = 3, ErrorMessage = "La matrícula debe tener entre 3 y 8 caracteres")]
        public string matriculaAvion { get; set; }

        [Required(ErrorMessage = "La capacidad es requerida")]
        [Range(10, 200, ErrorMessage = "La capacidad debe ser un número entre 10 y 200 pasajeros")]
        public int capacidadAvion { get; set; }

        [Required(ErrorMessage = "La clase del avión es requerida")]
        public string claseAvion { get; set; }
    }
} 