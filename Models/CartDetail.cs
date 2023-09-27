using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public class CartDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? CartDetailId { get; set; }
        public string? CartId { get; set; }
        public int? ProductItemId { get; set; }
        public int? DogItemId { get; set; }
        public short Quantity { get; set; }


        public Cart Cart { get; set; } = null!;

        //public List<DogItem> dogItems  { get; } = new();
        //public List<DogProductItem> dogProductItems { get; } = new();

        public List<Inventory> inventories { get; } = new();

    }
}
