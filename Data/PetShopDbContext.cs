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
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartDetail> CartDetail { get; set; }
        public DbSet<DogItem> DogItem { get; set; }
        public DbSet<DogProductItem> DogProductItem { get; set; }
        public DbSet<DogProductType> DogProductType { get; set; }
        public DbSet<DogSpecies> DogSpecies { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<ShipInfo> ShipInfo { get; set; }
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

                 
        }
    }
}
