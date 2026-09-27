using Classwork_27_09_2026.DTOs;
using Classwork_27_09_2026.Services.Abstract;
using System.Xml.Linq;

namespace Classwork_27_09_2026.Services;

public class ProductService : IProductService
{
    private static readonly List<ProductDto> products = new List<ProductDto>
    {
        new ProductDto{Id = Guid.Parse("alb2c3d4-e5f6-7890-abcd-ef1234567890"), Name = "Laptop", Price = 1000 },
        new ProductDto{Id = Guid.Parse("b2c3d4e5-f6a7-8901-bcde-f12345678901"), Name = "Computer", Price = 1500}
    };
    public ProductDto Create(CreateProductDto product)
    {
        var newProduct = new ProductDto
        {
            Id = Guid.NewGuid(),
            Name = product.Name,
            Price = product.Price
        };
        products.Add(newProduct);
        return newProduct;
    }

    public void Delete(Guid id)
    {
        var product = GetById(id);
        products.Remove(product);
    }

    public List<ProductDto> GetAll()
    {
        return products;
    }

    public ProductDto GetById(Guid id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            throw new Exception($"Product not found by {id}");
        }
        return product;
    }

    public void Update(Guid id, UpdateProductDto product)
    {
        var updatedProduct = GetById(id);
        updatedProduct.Name = product.Name;
        updatedProduct.Price = product.Price;
    }
}
