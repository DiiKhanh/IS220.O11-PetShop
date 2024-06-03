using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using PetShop.Models;

namespace PetShop.Data
{
    public class ApplicationUser: IdentityUser
    { 
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string? VerificationToken { get; set; } = string.Empty;
        public DateTime? VerifiedAt { get; set; }
        public string? AvatarUrl { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
