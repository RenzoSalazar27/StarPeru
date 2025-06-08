using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AeroLinea.Models
{
    [Table("iam_account")]
    public class Pasajero
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long id { get; set; }

        [Required(ErrorMessage = "El DNI es requerido")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener exactamente 8 dígitos")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El DNI solo debe contener números")]
        [Column("sender_id", TypeName = "varchar(8)")]
        public string sender_id { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "El nombre debe tener entre 5 y 30 caracteres")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre solo debe contener letras y espacios")]
        [Column("name", TypeName = "varchar(100)")]
        public string name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "El apellido debe tener entre 5 y 30 caracteres")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El apellido solo debe contener letras y espacios")]
        [Column("lastname", TypeName = "varchar(100)")]
        public string lastname { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [StringLength(1000)]
        [Column("email", TypeName = "varchar(1000)")]
        public string email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 20 caracteres")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[\W_]).+$", ErrorMessage = "Debe contener letras, números y caracteres especiales")]
        [Column("password", TypeName = "varchar(1000)")]
        public string password { get; set; } = string.Empty;

        [Column("flag_admin", TypeName = "int")]
        public int flag_admin { get; set; }

        [Column("flag_active", TypeName = "int")]
        public int flag_active { get; set; }

        [Column("created_at", TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column("deleted_at", TypeName = "timestamp")]
        public DateTime? deleted_at { get; set; }


        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [Column("fecha_nacimiento", TypeName = "date")]
        public DateTime? fechaNacimiento { get; set; }

        [Required(ErrorMessage = "El número de teléfono es obligatorio")]
        [RegularExpression(@"^9\d{8}$", ErrorMessage = "Debe ser un número de 9 dígitos que comience con 9")]
        [Column("telefono", TypeName = "varchar(9)")]
        public string telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de documento es obligatorio")]
        [RegularExpression(@"^(DNI|Pasaporte)$", ErrorMessage = "Debe ser 'DNI' o 'Pasaporte'")]
        [Column("tipo_documento", TypeName = "varchar(20)")]
        public string tipoDocumento { get; set; } = string.Empty;

        [Column("discapacidad", TypeName = "tinyint(1)")]
        public bool discapacidad { get; set; }

        [Column("tipo_discapacidad", TypeName = "varchar(255)")]
        public string? tipoDiscapacidad { get; set; }
    }
}