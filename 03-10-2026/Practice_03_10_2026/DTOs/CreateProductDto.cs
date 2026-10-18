namespace Practice_03_10_2026.DTOs;

/// <summary>
/// DTO для створення нового товару
/// </summary>
public class CreateProductDto
{
    /// <summary>Назва товару</summary>
    /// <example>Бездротова клавіатура</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>Ціна товару</summary>
    /// <example>1299.99</example>
    public decimal Price { get; set; }
}
