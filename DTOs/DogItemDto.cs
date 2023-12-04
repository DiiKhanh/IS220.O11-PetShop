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
        public int Age { get; set; }
        public string Origin { get; set; }
        public string HealthStatus { get; set; }
        public string Description { get; set; }
        public string[] Images { get; set; }
        public bool? IsInStock { get; set; }
        public bool? IsDeleted { get; set; }
    }
<<<<<<< HEAD
    public class DogItemUpdateRequest
=======
    public class DogItemDtoUpdate
>>>>>>> 3cb2bee2ef48e6672679d62ae9d5ee7f59d87b50
    {
        public string? DogName { get; set; }
        public string? SpeciesName { get; set; }
        public int? Price { get; set; }
        public string? Color { get; set; }
        public string? Sex { get; set; }
        public int? Age { get; set; }
        public string? Origin { get; set; }
        public string? HealthStatus { get; set; }
        public string? Description { get; set; }
        public string[]? Images { get; set; }
        public bool? IsInStock { get; set; }
        public bool? IsDeleted { get; set; }
    }
<<<<<<< HEAD
=======

    public class DogItemResponse
    {
        public int? DogItemId { get; set; }
        public string DogName { get; set; }
        public int Price { get; set; }
        public string Color { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public string Origin { get; set; }
        public string HealthStatus { get; set; }
        public string Description { get; set; }
        public string[]? Images { get; set; }
        public bool? IsInStock { get; set; }
        public bool? IsDeleted { get; set; }

        public int DogSpeciesId { get; set; }
    }
>>>>>>> 3cb2bee2ef48e6672679d62ae9d5ee7f59d87b50
}
