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

        public ICollection<DogItem> dogItems  { get; set; } 
        public ICollection<DogProductItem> dogProductItems { get; set; } 

    }
}
