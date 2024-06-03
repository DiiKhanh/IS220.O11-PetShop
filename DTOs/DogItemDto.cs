using PetShop.Models;
using System.ComponentModel.DataAnnotations;

namespace PetShop.DTOs
{
    public class DogItemDto
    {
        [Required]
        public string DogName { get; set; }
        [Required]
        public string SpeciesName { get; set; }
        [Required]
        public int Price { get; set; }
        public string Color { get; set; }
        public string Sex { get; set; }
        public string Type {  get; set; }
        public int Age { get; set; }
        public string Origin { get; set; }
        public string HealthStatus { get; set; }
        public string Description { get; set; }
        public string[] Images { get; set; }
        public bool? IsInStock { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
