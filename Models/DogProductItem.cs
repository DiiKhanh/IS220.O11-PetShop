using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public class DogProductItem : DateEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? DogProductItemId { get; set; }
        public string ItemName { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string? Images { get; set; }
        public int Quantity { get; set; }
        public bool? IsInStock { get; set; }
        public bool? IsDeleted { get; set; }


        public ICollection<OrderDetail> orderDetails { get; set; } = new List<OrderDetail>();

        public ICollection<CartDetail> cartDetails { get; set; } 


    }
}
