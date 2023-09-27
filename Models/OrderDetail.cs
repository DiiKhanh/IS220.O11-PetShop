using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public class OrderDetail : DateEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? OrderDetailId { get; set; }
        public int? OrderId { get; set; }
        public int? ItemId { get; set; }
        public short Quantity { get; set; }
        public bool? IsDeleted { get; set; }

       

        public Order Order { get; set; } = null!;

        public DogItem DogItem { get; set; } = null!;
        public DogProductItem DogProductItem { get; set; } = null!;

    }
}
