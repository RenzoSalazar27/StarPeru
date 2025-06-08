using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AeroLinea.Models
{
    [Table("iam_account")]
    public class IamAccount
    {
        [Key]
        public int id { get; set; }

        [Required]
        [Column("sender_id")]
        public string SenderId { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string lastname { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        public string password { get; set; }

        [Column("flag_admin")]
        public bool FlagAdmin { get; set; } = false;

        [Column("flactive")]
        public bool FlActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
} 