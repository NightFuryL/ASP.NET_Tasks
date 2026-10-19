using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Practice_17_10_2026.Data;
using Practice_17_10_2026.Data.Models;
using Practice_17_10_2026.DTO.Order;
using Practice_17_10_2026.DTO.Pagination;

namespace Practice_17_10_2026.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public OrderService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OrderDTO> CreateOrderAsync(CreateOrderDTO dto)
    {
        var order = _mapper.Map<Order>(dto);
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return _mapper.Map<OrderDTO>(order);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return false;

        order.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<OrderDTO?> GetOrderByIdAsync(Guid id)
    {
        var order = await _context.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return null;

        return _mapper.Map<OrderDTO>(order);
    }

    public async Task<PageResponse<OrderDTO>> GetPagedAsync(OrderQueryParams queryParams)
    {
        var query = _context.Orders.AsNoTracking().Where(o => !o.IsDeleted).AsQueryable();

        // Searching
        if (!string.IsNullOrWhiteSpace(queryParams.Search))
        {
            var search = queryParams.Search.Trim().ToLower();
            query = query.Where(o =>
                o.OrderNumber.ToLower().Contains(search) ||
                o.CustomerName.ToLower().Contains(search));
        }

        // Filtering
        if (!string.IsNullOrWhiteSpace(queryParams.Status))
        {
            var status = queryParams.Status.Trim().ToLower();
            query = query.Where(o => o.Status.ToLower() == status);
        }

        if (queryParams.MinTotal.HasValue)
        {
            query = query.Where(o => o.TotalAmount >= queryParams.MinTotal.Value);
        }

        if (queryParams.MaxTotal.HasValue)
        {
            query = query.Where(o => o.TotalAmount <= queryParams.MaxTotal.Value);
        }

        // Sorting
        query = queryParams.SortBy?.ToLower() switch
        {
            "ordernumber" => queryParams.IsSortDescending ? query.OrderByDescending(o => o.OrderNumber) : query.OrderBy(o => o.OrderNumber),
            "customername" => queryParams.IsSortDescending ? query.OrderByDescending(o => o.CustomerName) : query.OrderBy(o => o.CustomerName),
            "totalamount" => queryParams.IsSortDescending ? query.OrderByDescending(o => o.TotalAmount) : query.OrderBy(o => o.TotalAmount),
            "status" => queryParams.IsSortDescending ? query.OrderByDescending(o => o.Status) : query.OrderBy(o => o.Status),
            "createdat" => queryParams.IsSortDescending ? query.OrderByDescending(o => o.CreatedAt) : query.OrderBy(o => o.CreatedAt),
            _ => queryParams.IsSortDescending ? query.OrderByDescending(o => o.CreatedAt) : query.OrderBy(o => o.CreatedAt)
        };

        // Pagination
        var totalCount = await query.CountAsync();
        var items = await query.Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .ToListAsync();

        var mappedItems = _mapper.Map<List<OrderDTO>>(items);

        return new PageResponse<OrderDTO>(mappedItems, totalCount, queryParams.PageNumber, queryParams.PageSize);
    }
}
