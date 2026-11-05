using AspNetAssignments.Models.Order;
using AspNetAssignments.Models.Product;
using AspNetAssignments.Models.User;
using Microsoft.EntityFrameworkCore;
#region Додаткові сутності
using AspNetAssignments.Models.Book;
using AspNetAssignments.Models.Invoice;
using AspNetAssignments.Models.Project;
#endregion
namespace AspNetAssignments.Data;

/// <summary>
/// Контекст бази даних Entity Framework Core для доступу до таблиць системи
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    #region Додаткові сутності
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Book> Books => Set<Book>();
    #endregion

    /// <summary>
    /// Налаштування конфігурації сутностей, зв'язків та глобальних фільтрів
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Конфігурація сутності Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(p => p.Description).HasMaxLength(500);
            entity.Property(p => p.Quantity).IsRequired();
            entity.Property(p => p.CreatedDate).IsRequired();
            entity.Property(p => p.IsDeleted).IsRequired();

            // Фільтрація м'яко видалених записів
            entity.HasQueryFilter(p => !p.IsDeleted);

            entity.HasMany(p => p.Images)
                  .WithOne(i => i.Product)
                  .HasForeignKey(i => i.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Конфігурація сутності ProductImage
        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(pi => pi.Id);
            entity.Property(pi => pi.ImageUrl).IsRequired().HasMaxLength(500);

            entity.HasQueryFilter(pi => !pi.Product.IsDeleted);
        });


        // Конфігурація сутності User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Role).HasMaxLength(50);
            entity.Property(u => u.AvatarURL).HasMaxLength(500);
            entity.Property(u => u.IsDeleted).IsRequired();

            // Фільтрація м'яко видалених записів
            entity.HasQueryFilter(u => !u.IsDeleted);
        });

        // Конфігурація сутностей Order та OrderItem
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.OrderNumber).IsRequired().HasMaxLength(50);
            entity.Property(o => o.CreatedAt).IsRequired();
            entity.Property(o => o.IsDeleted).IsRequired();

            entity.HasMany(o => o.Items)
                  .WithOne(i => i.Order)
                  .HasForeignKey(i => i.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasQueryFilter(o => !o.IsDeleted);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.ProductName).IsRequired().HasMaxLength(150);
            entity.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");
        });

        #region Додаткові сутності
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
        #endregion
    }
}
