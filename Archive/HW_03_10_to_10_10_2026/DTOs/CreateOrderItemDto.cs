namespace HW_03_10_to_10_10_2026.DTOs;

/// <summary>
/// DTO для створення позиції замовлення
/// </summary>
public class CreateOrderItemDto
{
    /// <summary>Ідентифікатор товару</summary>
    /// <example>1</example>
    public int ProductId { get; set; }

    /// <summary>Кількість товару</summary>
    /// <example>2</example>
    public int Quantity { get; set; }
}
