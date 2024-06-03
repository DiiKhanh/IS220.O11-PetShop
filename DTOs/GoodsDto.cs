using System.ComponentModel.DataAnnotations;

namespace PetShop.DTOs
{
    public class GoodsDto
    {
        [Required]
        public string ProductName { get; set; }
        public int Price { get; set; }
        [Required]
        public string Supplier { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public int Quantity { get; set; }
        public string Address { get; set; }
        public string? Note { get; set; }
        public int? Total { get; set; }

    }
}
