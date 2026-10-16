namespace Practice_03_10_2026.Models;

/// <summary>
/// Елемент позиції замовлення
/// </summary>
public class OrderItem
{
    /// <summary>Унікальний ідентифікатор позиції</summary>
    public int Id { get; set; }

    /// <summary>Ідентифікатор товару</summary>
    public int ProductId { get; set; }

    /// <summary>Назва товару</summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>Ціна за одиницю</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>Кількість одиниць</summary>
    public int Quantity { get; set; }

    /// <summary>Загальна вартість позиції</summary>
    public decimal Total => UnitPrice * Quantity;
}
