using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PetShop.Models;

namespace PetShop.Data
{
    public class PetShopDbContext: IdentityDbContext<ApplicationUser>
    {
        public PetShopDbContext(DbContextOptions<PetShopDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityUser>(o =>
            {
                o.HasIndex(u => u.PhoneNumber).IsUnique();
            }
            );

            builder.Entity<DogItem>()
                   .HasMany(e => e.cartDetails)
                   .WithMany(e => e.dogItems)
                   .UsingEntity(
                        "DogItemInventory",
                        l => l.HasOne(typeof(CartDetail)).WithMany().HasForeignKey("CartDetailId").HasPrincipalKey(nameof(CartDetail.CartDetailId)),
                        r => r.HasOne(typeof(DogItem)).WithMany().HasForeignKey("DogItemId").HasPrincipalKey(nameof(DogItem.DogItemId)),
                        j => j.HasKey("CartDetailId", "DogItemId"));

            builder.Entity<DogProductItem>()
                   .HasMany(e => e.cartDetails)
                   .WithMany(e => e.dogProductItems)
                   .UsingEntity(
                        "DogItemInventory",
                        l => l.HasOne(typeof(CartDetail)).WithMany().HasForeignKey("CartDetailId").HasPrincipalKey(nameof(CartDetail.CartDetailId)),
                        r => r.HasOne(typeof(DogProductItem)).WithMany().HasForeignKey("DogProductItemId").HasPrincipalKey(nameof(DogProductItem.DogProductItemId)),
                        j => j.HasKey("CartDetailId", "DogProductItemId"));
        }
    }
}
