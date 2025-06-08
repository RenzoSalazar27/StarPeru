using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AeroLinea.Models
{
    public class RegisteredFlight
    {
        [Key]
        public int id { get; set; }
        public string number { get; set; }
        public int type_fly_id { get; set; }
        public int origin_id { get; set; }
        public int destiny_id { get; set; }
        public DateTime time_start { get; set; }
        public string flag_active { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? deleted_at { get; set; }
    }
} 