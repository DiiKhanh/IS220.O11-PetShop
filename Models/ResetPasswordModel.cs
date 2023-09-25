using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
    public class ResetPasswordModel
    {
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required, MinLength(3, ErrorMessage = "The password needs to be at least 3 characters.")]
        public string NewPassword { get; set; } = string.Empty;
        [Required, Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
