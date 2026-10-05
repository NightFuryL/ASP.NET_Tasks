namespace Practice_26_09_2026.DTOs;

public class CreateOrderItemDto
{
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
