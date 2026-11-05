using System.ComponentModel.DataAnnotations;

namespace AspNetAssignments.DTOs.Invoice;

public class InvoiceReadDto
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class InvoiceCreateDto
{
    [Required(ErrorMessage = "ClientName обов'язковий.")]
    [MaxLength(150)]
    public string ClientName { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 10000000.0, ErrorMessage = "Сума рахунку (Total) повинна бути більшою за нуль.")]
    public decimal Total { get; set; }
}

public class InvoiceUpdateDto
{
    [Required(ErrorMessage = "ClientName обов'язковий.")]
    [MaxLength(150)]
    public string ClientName { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 10000000.0, ErrorMessage = "Сума рахунку (Total) повинна бути більшою за нуль.")]
    public decimal Total { get; set; }
}
