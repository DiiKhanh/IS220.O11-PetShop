using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public class Goods : DateEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? GoodsId { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public string Supplier { get; set; }
        public string PhoneNumber { get; set; }
        public int Quantity { get; set; }
        public string Address { get; set; }
        public string? Note { get; set; }
        public int? Total { get; set; }

    }
}
