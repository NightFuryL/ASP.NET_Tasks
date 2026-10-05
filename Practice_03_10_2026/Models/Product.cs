namespace Practice_03_10_2026.Models;

/// <summary>
/// Сутність товару в системі
/// </summary>
public class Product
{
    /// <summary>Унікальний ідентифікатор товару</summary>
    public int Id { get; set; }

    /// <summary>Назва товару</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Ціна товару</summary>
    public decimal Price { get; set; }
}
