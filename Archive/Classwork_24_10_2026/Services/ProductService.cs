using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Classwork_24_10_2026.Data;
using Classwork_24_10_2026.Data.Models;
using Classwork_24_10_2026.DTO.Product;
using Classwork_24_10_2026.DTO.Pagination;

namespace Classwork_24_10_2026.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductDTO> CreateProductAsync(CreateProductDTO dto)
        {
            var product = _mapper.Map<Product>(dto);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return false;

            product.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _context.Products.Where(p => !p.IsDeleted).AsNoTracking().ToListAsync();
            return _mapper.Map<List<ProductDTO>>(products);
        }

        public async Task<PageResponse<ProductDTO>> GetPagedAsync(ProductQueryParams queryParams)
        {
            var query = _context.Products.AsNoTracking().Where(p => !p.IsDeleted).AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParams.Name))
            {
                var search = queryParams.Name.Trim().ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(search) ||
                    (p.Description != null && p.Description.ToLower().Contains(search)));
            }

            if (queryParams.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= queryParams.MinPrice.Value);
            }

            if (queryParams.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= queryParams.MaxPrice.Value);
            }

            query = queryParams.SortBy?.ToLower() switch
            {
                "name" => queryParams.IsSortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                "price" => queryParams.IsSortDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                "createddate" => queryParams.IsSortDescending ? query.OrderByDescending(p => p.CreatedDate) : query.OrderBy(p => p.CreatedDate),
                _ => queryParams.IsSortDescending ? query.OrderByDescending(p => p.CreatedDate) : query.OrderBy(p => p.CreatedDate)
            };

            var totalCount = await query.CountAsync();
            var items = await query.Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            var mappedItems = _mapper.Map<List<ProductDTO>>(items);

            return new PageResponse<ProductDTO>(mappedItems, totalCount, queryParams.PageNumber, queryParams.PageSize);
        }

        public async Task<ProductDTO?> GetProductByIdAsync(Guid id)
        {
            var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return null;

            return _mapper.Map<ProductDTO>(product);
        }
    }
}
