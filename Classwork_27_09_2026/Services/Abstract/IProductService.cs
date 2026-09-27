using Classwork_27_09_2026.DTOs;
using System.Security.Cryptography.Xml;

namespace Classwork_27_09_2026.Services.Abstract;
public interface IProductService
{
    List < ProductDto > GetAll();
    ProductDto GetById(Guid id);
    ProductDto Create(CreateProductDto product);
    void Update(Guid id, UpdateProductDto product);
    void Delete(Guid id);
}
