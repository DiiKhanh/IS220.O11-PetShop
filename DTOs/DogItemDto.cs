using System.ComponentModel.DataAnnotations;

namespace PetShop.DTOs
{
    public class DogItemDto
    {
        [Required]
        public string DogName { get; set; }
        [Required]
        public int DogSpeciesId { get; set; }
        [Required]
        public int Price { get; set; }
        public string Color { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public string Origin { get; set; }
        public string HealthStatus { get; set; }
        public string Description { get; set; }
        public byte[] Images { get; set; }
    }
}
