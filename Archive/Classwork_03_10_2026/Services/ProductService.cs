using Classwork_03_10_2026.DTO;
using Classwork_03_10_2026.Services.Abstract;
using System.Xml.Linq;

namespace Classwork_03_10_2026.Services;

public class ProductService : IProductService
{
    private static readonly List<CreateProductDTO> products = new List<CreateProductDTO>();
    public CreateProductDTO Create(CreateProductDTO product)
    {
        var newProduct = new CreateProductDTO
        {
            Name = product.Name,
            Price = product.Price
        };
        products.Add(newProduct);
        return newProduct;
    }
}
