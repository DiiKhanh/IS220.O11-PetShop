using PetShop.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.DTOs
{
    public class DogProductItemDto
    {
        [Required]
        public string ItemName { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string[] Images { get; set; }
        public int Quantity { get; set; }
        public bool? IsInStock { get; set; }
        public bool? IsDeleted { get; set; }

    }

    public class DogProductItemResponse
    {
        public int? DogProductItemId { get; set; }
        public string ItemName { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string[] Images { get; set; }
        public int Quantity { get; set; }
        public bool? IsInStock { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
