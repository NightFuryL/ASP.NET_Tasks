using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AspNetAssignments.Data;
using AspNetAssignments.DTOs.Pagination;
using AspNetAssignments.DTOs.Product;
using AspNetAssignments.Models.Product;
using AspNetAssignments.Services.Abstract;

namespace AspNetAssignments.Services;

/// <summary>
/// Сервіс для управління даними товарів через Entity Framework Core
/// </summary>
public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ProductService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Отримує товари з підтримкою фільтрації за ціною, пошуку та сортування
    /// </summary>
    public async Task<PageResponse<ProductDTO>> GetPagedAsync(ProductQueryParams queryParams)
    {
        var query = _context.Products.AsNoTracking().Where(p => !p.IsDeleted).AsQueryable();

        // Пошук за назвою або описом
        if (!string.IsNullOrWhiteSpace(queryParams.Name))
        {
            var search = queryParams.Name.Trim().ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(search) ||
                (p.Description != null && p.Description.ToLower().Contains(search)));
        }

        // Фільтрація за ціною
        if (queryParams.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= queryParams.MinPrice.Value);
        }

        if (queryParams.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= queryParams.MaxPrice.Value);
        }

        // Сортування результатів
        query = queryParams.SortBy?.ToLower() switch
        {
            "name" => queryParams.IsSortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
            "price" => queryParams.IsSortDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
            "createddate" => queryParams.IsSortDescending ? query.OrderByDescending(p => p.CreatedDate) : query.OrderBy(p => p.CreatedDate),
            _ => queryParams.IsSortDescending ? query.OrderByDescending(p => p.CreatedDate) : query.OrderBy(p => p.CreatedDate)
        };

        // Пагінація
        var totalCount = await query.CountAsync();
        var items = await query.Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                               .Take(queryParams.PageSize)
                               .ToListAsync();

        var mappedItems = _mapper.Map<List<ProductDTO>>(items);

        return new PageResponse<ProductDTO>(mappedItems, totalCount, queryParams.PageNumber, queryParams.PageSize);
    }

    /// <summary>
    /// Отримує всі активні товари без пагінації
    /// </summary>
    public async Task<List<ProductDTO>> GetAllProductsAsync()
    {
        var products = await _context.Products
            .AsNoTracking()
            .Where(p => !p.IsDeleted)
            .ToListAsync();

        return _mapper.Map<List<ProductDTO>>(products);
    }

    /// <summary>
    /// Отримує дані товару за його ідентифікатором
    /// </summary>
    public async Task<ProductDTO?> GetProductByIdAsync(Guid id)
    {
        var product = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        return product == null ? null : _mapper.Map<ProductDTO>(product);
    }

    /// <summary>
    /// Додає новий товар до бази даних
    /// </summary>
    public async Task<ProductDTO> CreateProductAsync(CreateProductDTO dto)
    {
        var product = _mapper.Map<Product>(dto);
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return _mapper.Map<ProductDTO>(product);
    }

    /// <summary>
    /// Оновлює дані наявного товару за ідентифікатором
    /// </summary>
    public async Task<ProductDTO?> UpdateProductAsync(Guid id, UpdateProductDTO dto)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        if (product == null) return null;

        product.Name = dto.Name;
        product.Price = dto.Price;
        product.Description = dto.Description;
        product.Quantity = dto.Quantity;

        await _context.SaveChangesAsync();
        return _mapper.Map<ProductDTO>(product);
    }

    /// <summary>
    /// Виконує м'яке видалення товару, встановлюючи IsDeleted = true
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        if (product == null) return false;

        product.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    #region Alternative In-Memory Implementation
    /*
    // Альтернативна in-memory версія збереження без бази даних
    private static readonly List<ProductDTO> _inMemoryProducts = new()
    {
        new ProductDTO { Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), Name = "Laptop", Price = 1000, Quantity = 10, CreatedDate = DateTime.UtcNow },
        new ProductDTO { Id = Guid.Parse("b2c3d4e5-f6a7-8901-bcde-012345678901"), Name = "Computer", Price = 1500, Quantity = 5, CreatedDate = DateTime.UtcNow }
    };

    public List<ProductDTO> GetAllInMemory() => _inMemoryProducts;

    public ProductDTO CreateInMemory(CreateProductDTO dto)
    {
        var newProduct = new ProductDTO
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Price = dto.Price,
            Quantity = dto.Quantity,
            Description = dto.Description,
            CreatedDate = DateTime.UtcNow
        };
        _inMemoryProducts.Add(newProduct);
        return newProduct;
    }
    */
    #endregion
}
