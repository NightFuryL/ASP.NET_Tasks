using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AspNetAssignments.Models.User;

namespace AspNetAssignments.Models.Order;

/// <summary>
/// Сутність замовлення клієнта
/// </summary>
public class Order
{
    public int Id { get; set; }

    /// <summary>
    /// Ідентифікатор користувача, який оформив замовлення
    /// </summary>
    public Guid? UserId { get; set; }

    [ForeignKey("UserId")]
    public User.User? User { get; set; }

    [Required]
    [MaxLength(50)]
    public string OrderNumber { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<OrderItem> Items { get; set; } = new();

    public decimal TotalAmount => Items.Sum(i => i.Total);

    public bool IsDeleted { get; set; } = false;
}
