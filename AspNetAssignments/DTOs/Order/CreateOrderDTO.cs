using System.ComponentModel.DataAnnotations;

namespace AspNetAssignments.DTOs.Order;

public class CreateOrderDTO
{
    [Required(ErrorMessage = "Номер замовлення є обов'язковим.")]
    [MaxLength(50)]
    public string OrderNumber { get; set; } = string.Empty;

    public List<CreateOrderItemDTO> Items { get; set; } = new();
}
