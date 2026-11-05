namespace AspNetAssignments.DTOs.Product;

public class ProductDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<string> Images { get; set; } = new();
    public bool IsDeleted { get; set; }
}
