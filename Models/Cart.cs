using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public class Cart
    {
        [Key]
        public string? CartId { get; set; }
        public int? Total { get; set; }

        public ICollection<CartDetail> cartDetails { get; set; } = new List<CartDetail>();
    }
}
