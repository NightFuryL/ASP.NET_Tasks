using System.ComponentModel.DataAnnotations;

namespace Classwork_03_10_2026.DTO;
/// <summary>
/// DTO for creating a product
/// </summary>
public class CreateProductDTO
{
    /// <summary>Product name</summary>
    /// <example>Laptop Lenovo</example>
    [Required]
    public string Name { get; set; } = null!;
    /// <summary>Price in dollars</summary>
    /// <example>999.99$</example>
    [Required, Range(0, 1000000, ErrorMessage = "Price must be a positive value")]
    public decimal Price { get; set; }
    /// <summary>Product description</summary>
    /// <example>High-performance laptop for gaming and productivity</example>
    public string? Description { get; set; }
}
