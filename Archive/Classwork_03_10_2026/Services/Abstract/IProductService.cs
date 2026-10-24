using Classwork_03_10_2026.DTO;
using System.Security.Cryptography.Xml;

namespace Classwork_03_10_2026.Services.Abstract;
public interface IProductService
{
    CreateProductDTO Create(CreateProductDTO product);
}
