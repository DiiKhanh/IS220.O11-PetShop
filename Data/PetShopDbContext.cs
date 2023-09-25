using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        }
    }
}
