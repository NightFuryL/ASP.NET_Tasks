using AutoMapper;
using Classwork_04_10_2026.DataBase;
using Classwork_04_10_2026.DTO;
using Classwork_04_10_2026.Services.Abstract;
using System.Xml.Linq;

namespace Classwork_04_10_2026.Services;

public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private static readonly List<Product> _database = new List<Product>() 
    {
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Product 1",
            Price = 10.99m,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Product 2",
            Price = 19.99m,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        }
    };
    public ProductService(IMapper mapper)
    {
        _mapper = mapper;
    }
    public ProductDTO Create(CreateProductDTO product)
    {
        var newProduct = _mapper.Map<Product>(product);
        _database.Add(newProduct);
        return _mapper.Map<ProductDTO>(newProduct);
    }

    public List<Product> GetProducts()
    {
        var productDTOs = _database
                .Where(p => !p.IsDeleted)
                .Select(p => _mapper.Map<Product>(p))
                .ToList();
        return productDTOs;
    }
}
