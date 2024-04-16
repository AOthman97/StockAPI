using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base (dbContextOptions)
        {
            
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Portfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));

            // In here and the below, Define the many to many relationship between the Stock and AppUser in the joint table
            // "Portfolios"
            builder.Entity<Portfolio>()
                .HasOne(u => u.AppUser)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.AppUserId);

            builder.Entity<Portfolio>()
               .HasOne(u => u.Stock)
               .WithMany(u => u.Portfolios)
               .HasForeignKey(p => p.StockId);

            // * AFTER CREATING THIS DON'T FORGET TO SEED THE DATABASE BY CREATING A NEW MIGRATION AND UPDATING THE
            // DATABASE SO THESE ROLES CAN BE APPLIED AND USED
            // Create the roles
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                }
            };

            // Then add the roles to the db
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}