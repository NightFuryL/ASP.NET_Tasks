using System.ComponentModel.DataAnnotations;

namespace AspNetAssignments.DTOs.Order;

public class CreateOrderDTO
{
    public Guid? UserId { get; set; }

    [Required(ErrorMessage = "Номер замовлення є обов'язковим.")]
    [MaxLength(50)]
    public string OrderNumber { get; set; } = string.Empty;

    public List<CreateOrderItemDTO> Items { get; set; } = new();
}
