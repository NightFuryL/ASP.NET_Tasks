namespace Classwork_04_10_2026.DataBase;
public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
