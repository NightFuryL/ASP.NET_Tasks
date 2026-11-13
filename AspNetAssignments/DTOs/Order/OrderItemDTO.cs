namespace AspNetAssignments.DTOs.Order;

public class OrderItemDTO
{
    public int Id { get; set; }
    public int? OrderId { get; set; }
    public Guid? ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Total => UnitPrice * Quantity;
}
