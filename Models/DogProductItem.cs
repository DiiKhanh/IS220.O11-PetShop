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
        public int? DogProductTypeId { get; set; }
        public string Description { get; set; }
        public byte[] Images { get; set; }
        public int Quantity { get; set; }
        public bool? IsInStock { get; set; }
        public bool? IsDeleted { get; set; }

        public DogProductType ProductType { get; set; } = null!;

        public ICollection<OrderDetail> orderDetails { get; set; } = new List<OrderDetail>();

        //public List<CartDetail> cartDetails { get; } = new();
        public List<Inventory> inventories { get; } = new();


    }
}
