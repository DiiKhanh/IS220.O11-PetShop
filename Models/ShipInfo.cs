using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public class ShipInfo
    {
        [Key]
        public int? ShipInfoId { get; set; }
        public string City { get; set; }

        public string Address { get; set; }

        public string District { get; set; }

        public Order? order { get; set; }

    }
}