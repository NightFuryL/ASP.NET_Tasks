using Classwork_11_10_2026.Models;
using Microsoft.EntityFrameworkCore;

namespace Classwork_11_10_2026.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Email)
            .IsRequired()

            .HasMaxLength(256);
            entity.HasIndex(e => e.Email)
            .IsUnique();

            entity.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

            entity.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

            entity.HasQueryFilter(e => !e.IsDeleted);
        });
    }   
}
