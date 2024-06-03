using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public class DogItem : DateEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? DogItemId { get; set; }
        public string DogName { get; set; }
        public int Price { get; set; }
        public string Color { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public string Type { get; set; }
        public string Origin { get; set; }
        public string HealthStatus { get; set; }
        public string Description { get; set; }
        public string? Images { get; set; }
        public bool? IsInStock { get; set; }
        public bool? IsDeleted { get; set; }

        public int DogSpeciesId { get; set; }
        public DogSpecies Species { get; set; } = null!;

        public ICollection<OrderDetail> orderDetails { get; set; } = new List<OrderDetail>();

        public ICollection<CartDetail> cartDetails { get; set; }  = new List<CartDetail>();

    }
}
