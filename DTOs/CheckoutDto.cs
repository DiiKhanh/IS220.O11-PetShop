using System.ComponentModel.DataAnnotations;

namespace PetShop.DTOs
{
    public class CheckoutDto
    {
        [Required]
        public string User_id { get; set; }
        [Required]
        public int Total { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public string Payment { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public DataObject[] Data { get; set; }
    }

    public class DataObject
    {
        public int id { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public string type { get; set; }
        public int price { get; set; }
        public int stock { get; set; }
        public int? totalPrice { get; set; }
        public string[] images { get; set; }
    }
}
