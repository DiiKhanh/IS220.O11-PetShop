using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }

        public int DogItemId { get; set; }
        public int DogProductItemId { get; set; }
        public List<DogItem> dog { get; set; } = null!;
        public List<DogProductItem> dogitem { get; set; } = null!;
        public List<CartDetail> details { get; set; } = null!;

    }
}
