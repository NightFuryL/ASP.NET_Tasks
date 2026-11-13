namespace AspNetAssignments.DTOs.Order;

public class OrderDTO
{
    public int Id { get; set; }
    public Guid? UserId { get; set; }
    public string? UserName { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<OrderItemDTO> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
}
