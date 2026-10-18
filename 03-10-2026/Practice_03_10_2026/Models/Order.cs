namespace Practice_03_10_2026.Models;

/// <summary>
/// Сутність замовлення
/// </summary>
public class Order
{
    /// <summary>Унікальний ідентифікатор замовлення</summary>
    public int Id { get; set; }

    /// <summary>Номер замовлення</summary>
    public string OrderNumber { get; set; } = string.Empty;

    /// <summary>Дата створення</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Список позицій замовлення</summary>
    public List<OrderItem> Items { get; set; } = new();

    /// <summary>Загальна сума замовлення</summary>
    public decimal TotalAmount => Items.Sum(i => i.Total);
}
