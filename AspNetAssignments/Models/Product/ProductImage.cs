namespace AspNetAssignments.Models.Product;

/// <summary>
/// Сутність фотографії товару
/// </summary>
public class ProductImage
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public string ImageUrl { get; set; } = string.Empty;
}
