using Microsoft.EntityFrameworkCore;
using Classwork_10_10_2026.Data.Models;

namespace Classwork_10_10_2026.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure the Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(p => p.Description).HasMaxLength(500);
                entity.Property(p => p.Quantity).IsRequired();
                entity.Property(p => p.CreatedDate).IsRequired();
                entity.Property(p => p.IsDeleted).IsRequired();
            });
        }
    }
       
}
