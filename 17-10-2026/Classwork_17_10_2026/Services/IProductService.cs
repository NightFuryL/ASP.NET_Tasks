using Classwork_17_10_2026.Data.Models;
using Classwork_17_10_2026.DTO.Pagination;
using Classwork_17_10_2026.DTO.Product;

namespace Classwork_17_10_2026.Services
{
    public interface IProductService
    {
        //Task<List<ProductDTO>> GetAllProductsAsync();
        Task<PageResponse<ProductDTO>> GetPagedAsync(ProductQueryParams queryParams);
        Task<ProductDTO?> GetProductByIdAsync(Guid id);
        Task<ProductDTO> CreateProductAsync(CreateProductDTO product);
        Task<bool> DeleteAsync(Guid id);
    }
}