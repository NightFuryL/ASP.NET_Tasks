namespace Practice_26_09_2026.Models;

public class OrderItem
{
    public Guid Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => Price * Quantity;
}
