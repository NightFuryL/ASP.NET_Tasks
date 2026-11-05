using System.ComponentModel.DataAnnotations;

namespace AspNetAssignments.Models.Invoice;

public class Invoice
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Number { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string ClientName { get; set; } = string.Empty;

    public decimal Total { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; } = false;
}
