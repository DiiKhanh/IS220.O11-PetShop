using PetShop.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.DTOs
{
    public class CartProductDto
    {
        public int DogProductItemId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public short Quantity { get; set; }

    }
}


