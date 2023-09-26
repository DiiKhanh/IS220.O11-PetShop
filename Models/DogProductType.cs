using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public class DogProductType : DateEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? DogProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public bool? IsDeleted { get; set; }

        public ICollection<DogProductItem> Items { get; set; } = new List<DogProductItem>();
    }
}
