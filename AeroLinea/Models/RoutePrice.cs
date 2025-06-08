using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AeroLinea.Models
{
    [Table("route_prices")]
    public class RoutePrice
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int OriginId { get; set; }
        
        [Required]
        public int DestinyId { get; set; }
        
        [Required]
        public decimal EconomyPrice { get; set; }
        
        [Required]
        public decimal ExecutivePrice { get; set; }
        
        [Required]
        public decimal PremiumPrice { get; set; }
        
        [ForeignKey("OriginId")]
        public Location Origin { get; set; }
        
        [ForeignKey("DestinyId")]
        public Location Destiny { get; set; }
    }
} 