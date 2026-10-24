namespace Practice_03_10_2026.DTOs;

/// <summary>
/// DTO для створення замовлення
/// </summary>
public class CreateOrderDto
{
    /// <summary>Номер замовлення</summary>
    /// <example>ORD-2026-101</example>
    public string OrderNumber { get; set; } = string.Empty;

    /// <summary>Список позицій замовлення</summary>
    public List<CreateOrderItemDto> Items { get; set; } = new();
}
