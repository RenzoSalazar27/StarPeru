using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace AeroLinea.Models
{
    [Table("reservaVuelos")]
    public class ReservaVuelo
    {
        [Key]
        public int idResVuelo { get; set; }

        [Required(ErrorMessage = "El origen es obligatorio")]
        [StringLength(15, ErrorMessage = "El origen no puede tener más de 15 caracteres")]
        public string origenResVue { get; set; }

        [Required(ErrorMessage = "El destino es obligatorio")]
        [StringLength(15, ErrorMessage = "El destino no puede tener más de 15 caracteres")]
        public string destinoVuelo { get; set; }

        [Required(ErrorMessage = "El número de pasajeros es obligatorio")]
        [Range(1, 10, ErrorMessage = "El número de pasajeros debe estar entre 1 y 10")]
        public int personasResVue { get; set; }

        [Required(ErrorMessage = "La información de mascotas es obligatoria")]
        public string mascotasResVue { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Required(ErrorMessage = "El precio total es obligatorio")]
        public decimal precioVuelo { get; set; }

        [Required(ErrorMessage = "El estado de pago es obligatorio")]
        public bool pagadoVuelo { get; set; }

        [Required(ErrorMessage = "El ID del vuelo es obligatorio")]
        public int idVuelo { get; set; }

        [ForeignKey("idVuelo")]
        public virtual Vuelo Vuelo { get; set; }

        [Required(ErrorMessage = "El ID del usuario es obligatorio")]
        public int idUsuario { get; set; }

        [ForeignKey("idUsuario")]
        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<Pasajero> Pasajeros { get; set; }
    }
} 