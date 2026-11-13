using AspNetAssignments.Entities.ProductRepo;

namespace AspNetAssignments.Repository.ProductRepo
{
    public interface IProductRepository
    {
        Task<ProductEntity?> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductEntity>> GetAllAsync();
        Task<ProductEntity> CreateAsync(ProductEntity product);
        Task<bool> DeleteAsync(Guid id);

    }
}
