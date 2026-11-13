using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AspNetAssignments.Models.Product;

namespace AspNetAssignments.Models.Order;

/// <summary>
/// Елемент позиції замовлення
/// </summary>
public class OrderItem
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    [ForeignKey("OrderId")]
    public Order? Order { get; set; }

    /// <summary>
    /// Ідентифікатор товару в системі
    /// </summary>
    public Guid? ProductId { get; set; }

    [ForeignKey("ProductId")]
    public Product.Product? Product { get; set; }

    [Required]
    [MaxLength(150)]
    public string ProductName { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal Total => UnitPrice * Quantity;
}
