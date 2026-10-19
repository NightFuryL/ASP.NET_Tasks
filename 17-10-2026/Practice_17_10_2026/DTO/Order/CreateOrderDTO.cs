using System.ComponentModel.DataAnnotations;

namespace Practice_17_10_2026.DTO.Order;

public class CreateOrderDTO
{
    [Required]
    public string OrderNumber { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "TotalAmount must be greater than zero.")]
    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = "Pending";
}
