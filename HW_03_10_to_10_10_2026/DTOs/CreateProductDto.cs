namespace HW_03_10_to_10_10_2026.DTOs;

/// <summary>
/// DTO для створення нового товару
/// </summary>
public class CreateProductDto
{
    /// <summary>Назва товару</summary>
    /// <example>Механічна клавіатура</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>Ціна товару</summary>
    /// <example>2499.00</example>
    public decimal Price { get; set; }
}
