namespace HW_04_10_to_12_10_2026.DTOs;

public class InvoiceReadDto
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public decimal Total { get; set; }
}
