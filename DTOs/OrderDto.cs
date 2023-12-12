using PetShop.Models;

namespace PetShop.DTOs
{
    public class OrderDto
    {
        public string? OrderStatus { get; set; }
        public string? ShipmentStatus { get; set; }
        public string? PaymentMethod { get; set; }
    }

    public class UpdateShipInfoDto
    {
        public string? City { get; set; }

        public string? Address { get; set; }

        public string? District { get; set; }
    }
}
