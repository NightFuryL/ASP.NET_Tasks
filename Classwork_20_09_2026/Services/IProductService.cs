using Classwork_20_09_2026.Models;
namespace Classwork_20_09_2026.Services;
public interface IProductService
{ 
    List<Product> GetAllProducts();
    Product? GetProductById(Guid id);
    Product CreateProduct(Product product);
    Product? UpdateProduct(Guid id, Product product);
    bool DeleteProduct(Guid id);
}