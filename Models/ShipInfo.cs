using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PetShop.Data;

namespace PetShop.Models
{
    public class ShipInfo
    {
        [Key]
        public int? ShipInfoId { get; set; }
        public string? UserId { get; set; }
        public string? City { get; set; }

        public string? Address { get; set; }

        public string? District { get; set; }

        public List<Order>? orders { get; set; }
        public ApplicationUser? user { get; set; } = null!;

    }
}