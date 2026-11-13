using System.ComponentModel.DataAnnotations;

namespace AspNetAssignments.DTOs.Order;

public class CreateOrderItemDTO
{
    public int? OrderId { get; set; }

    public Guid? ProductId { get; set; }

    [Required(ErrorMessage = "Назва товару є обов'язковою.")]
    [MaxLength(150)]
    public string ProductName { get; set; } = string.Empty;

    [Range(0.01, 1000000.0, ErrorMessage = "Ціна має бути більшою за нуль.")]
    public decimal UnitPrice { get; set; }

    [Range(1, 10000, ErrorMessage = "Кількість має бути більшою за нуль.")]
    public int Quantity { get; set; }
}
