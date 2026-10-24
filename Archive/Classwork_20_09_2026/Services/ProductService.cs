using Classwork_20_09_2026.Models;
namespace Classwork_20_09_2026.Services;
public class ProductService : IProductService
{
    private readonly List<Product> _products = new List<Product>() { 
        new Product { Id = Guid.NewGuid(), Name = "Laptop", Price = 80000m },
        new Product { Id = Guid.NewGuid(), Name = "Phone", Price = 20000m },
        new Product { Id = Guid.NewGuid(), Name = "Tablet", Price = 30000m }
    };
    public List<Product> GetAllProducts()
    {
        return _products;
    }
    public Product? GetProductById(Guid id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }
    public Product CreateProduct(Product product)
    {
        product.Id = Guid.NewGuid();
        _products.Add(product);
        return product;
    }
    public Product? UpdateProduct(Guid id, Product product)
    {
        var existingProduct = GetProductById(id);
        if (existingProduct == null)
        {
            return null;
        }
        existingProduct.Name = product.Name;
        existingProduct.Price = product.Price;
        return existingProduct;
    }
    public bool DeleteProduct(Guid id)
    {
        var product = GetProductById(id);
        if (product == null)
        {
            return false;
        }
        _products.Remove(product);
        return true;
    }

}
