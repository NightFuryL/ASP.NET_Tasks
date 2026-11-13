using AspNetAssignments.Data;
using AspNetAssignments.Entities.ProductRepo;
using Microsoft.EntityFrameworkCore;

namespace AspNetAssignments.Repository.ProductRepo;

public class ProductRepository : IProductRepository
{

    private readonly AppDbContext _context;
    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProductEntity> CreateAsync(ProductEntity ProductEntity)
    {
        ProductEntity.Id = Guid.NewGuid();
        ProductEntity.CreatedAt = DateTime.Now;

        await _context.ProductEntities.AddAsync(ProductEntity);
        await _context.SaveChangesAsync();
        return ProductEntity;
    }


    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) { return false; }
        product.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ProductEntity>> GetAllAsync()
    {
        return await _context.ProductEntities
            .AsNoTracking()
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

    }

    public async Task<ProductEntity?> GetByIdAsync(Guid id)
    {
        return await _context.ProductEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

}
