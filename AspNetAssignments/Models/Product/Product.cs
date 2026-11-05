using System.ComponentModel.DataAnnotations;

namespace AspNetAssignments.Models.Product;

/// <summary>
/// Сутність товару в системі
/// </summary>
public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public int Quantity { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public List<ProductImage> Images { get; set; } = new();

    public bool IsDeleted { get; set; } = false;
}
