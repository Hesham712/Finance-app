using api.Models;
using Finance_WebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Finance_WebApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Portfolio>(x=>x.HasKey(p=>new {p.AppUserId,p.StockId}));

            builder.Entity<Portfolio>()
                .HasOne(p=>p.Stock)
                .WithMany(p=>p.Portfolios)
                .HasForeignKey(p=>p.StockId);

            builder.Entity<Portfolio>()
                .HasOne(p => p.AppUser)
                .WithMany(p => p.Portfolios)
                .HasForeignKey(p => p.AppUserId);

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
            builder.Entity<IdentityRole>().HasData(roles);
        }
        //public DbSet<AppUser> AppUsers { get; set; }


    }
}
