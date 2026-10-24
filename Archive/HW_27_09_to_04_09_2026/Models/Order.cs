namespace HW_27_09_to_04_09_2026.Models;

public class Order
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
}
