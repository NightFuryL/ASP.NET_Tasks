using Classwork_25_10_2026.DTO.Pagination;
using Classwork_25_10_2026.DTO.Product;

namespace Classwork_25_10_2026.Services
{
    public interface IProductService
    {
        Task<PageResponse<ProductDTO>> GetPagedAsync(ProductQueryParams queryParams);
        Task<ProductDTO?> GetProductByIdAsync(Guid id);
        Task<ProductDTO> CreateProductAsync(CreateProductDTO product);
        Task<bool> DeleteAsync(Guid id);
    }
}
