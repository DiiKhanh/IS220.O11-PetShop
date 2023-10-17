using PetShop.Models;

namespace PetShop.DTOs
{
    public class CartDto
    {
        public List<int?> cartDetailId { get; set; }
        public short? Quantity { get; set; }
    }
}
