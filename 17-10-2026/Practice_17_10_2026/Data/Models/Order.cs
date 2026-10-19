using System.ComponentModel.DataAnnotations;

namespace Practice_17_10_2026.Data.Models;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string OrderNumber { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    [Required]
    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; } = false;
}
