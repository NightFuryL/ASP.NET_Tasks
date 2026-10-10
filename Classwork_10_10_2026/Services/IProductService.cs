using Classwork_10_10_2026.Data.Models;
using Classwork_10_10_2026.DTO;

namespace Classwork_10_10_2026.Services
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO?> GetProductByIdAsync(Guid id);
        Task<ProductDTO> CreateProductAsync(CreateProductDTO product);
        Task<bool> DeleteAsync(Guid id);
    }
}
