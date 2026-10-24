using Microsoft.EntityFrameworkCore;
using ExtraAspNetAssignments.Models.Book;
using ExtraAspNetAssignments.Models.Invoice;
using ExtraAspNetAssignments.Models.Project;

namespace ExtraAspNetAssignments.Data;

/// <summary>
/// Контекст бази даних Entity Framework Core для додаткових сутностей системи
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Book> Books => Set<Book>();

    /// <summary>
    /// Налаштування зв'язків та фільтрації м'якого видалення для сутностей
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Налаштування для проєктів та задач
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Description).HasMaxLength(500);
            entity.HasMany(p => p.Tasks)
                  .WithOne(t => t.Project)
                  .HasForeignKey(t => t.ProjectId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasQueryFilter(p => !p.IsDeleted);
        });

        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Title).IsRequired().HasMaxLength(150);
            entity.Property(t => t.Description).HasMaxLength(500);
            entity.HasQueryFilter(t => !t.IsDeleted);
        });

        // Налаштування для інвойсів
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Number).IsRequired().HasMaxLength(50);
            entity.Property(i => i.ClientName).IsRequired().HasMaxLength(150);
            entity.Property(i => i.Total).HasColumnType("decimal(18,2)");
            entity.HasQueryFilter(i => !i.IsDeleted);
        });

        // Налаштування для книг
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Title).IsRequired().HasMaxLength(150);
            entity.Property(b => b.Author).IsRequired().HasMaxLength(100);
            entity.HasQueryFilter(b => !b.IsDeleted);
        });
    }
}
