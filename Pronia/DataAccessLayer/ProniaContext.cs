using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pronia.Models;

namespace Pronia.DataAccessLayer
{
    public class ProniaContext : IdentityDbContext<AppUser>
    {
        public ProniaContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Order> Orders { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity baseEntity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            ((BaseEntity)entry.Entity).CreatedTime = DateTime.Now;
                            ((BaseEntity)entry.Entity).isDeleted = false;
                            break;
                        case EntityState.Modified:
                            ((BaseEntity)entry.Entity).UpdateTime = DateTime.Now;
                            break;
                            // Handle other cases like EntityState.Modified if needed
                    }
                }
            }

                return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ProniaDb;Trusted_Connection=True;TrustServerCertificate=True;"); 
            base.OnConfiguring(optionsBuilder);
        }

    }
}
