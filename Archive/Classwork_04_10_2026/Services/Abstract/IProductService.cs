using Classwork_04_10_2026.DataBase;
using Classwork_04_10_2026.DTO;
using System.Security.Cryptography.Xml;

namespace Classwork_04_10_2026.Services.Abstract;
public interface IProductService
{
    Product Create(CreateProductDTO product);
    List<ProductDTO> GetProducts();
}
