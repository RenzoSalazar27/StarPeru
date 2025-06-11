using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AeroLinea.Models
{
    public class Consulta
    {
        [Key]
        public int idConsulta { get; set; }

        [Required]
        [StringLength(15)]
        public string motivoConsulta { get; set; }

        [StringLength(200)]
        public string descripcionConsultas { get; set; }

        [Required]
        [StringLength(10)]
        public string urgencia { get; set; }

        [Required]
        [StringLength(10)]
        public string estado { get; set; }

        [StringLength(200)]
        public string? comentarioConsulta { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int idUsuario { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
} 