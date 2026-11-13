using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AspNetAssignments.Data;
using AspNetAssignments.DTOs.Order;
using AspNetAssignments.Models.Order;
using AspNetAssignments.Services.Abstract;

namespace AspNetAssignments.Services;

/// <summary>
/// Сервіс для управління замовленнями та позиціями замовлень через Entity Framework Core
/// </summary>
public class OrderService : IOrderService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public OrderService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Отримує всі замовлення разом із їхніми товарними позиціями та інформацією про клієнта
    /// </summary>
    public async Task<List<OrderDTO>> GetAllOrdersAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.Items)
            .Include(o => o.User)
            .AsNoTracking()
            .Where(o => !o.IsDeleted)
            .ToListAsync();

        return _mapper.Map<List<OrderDTO>>(orders);
    }

    /// <summary>
    /// Знаходить замовлення за його числовим ідентифікатором
    /// </summary>
    public async Task<OrderDTO?> GetOrderByIdAsync(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .Include(o => o.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);

        return order == null ? null : _mapper.Map<OrderDTO>(order);
    }

    /// <summary>
    /// Отримує всі замовлення конкретного користувача
    /// </summary>
    public async Task<List<OrderDTO>> GetOrdersByUserIdAsync(Guid userId)
    {
        var orders = await _context.Orders
            .Include(o => o.Items)
            .Include(o => o.User)
            .AsNoTracking()
            .Where(o => o.UserId == userId && !o.IsDeleted)
            .ToListAsync();

        return _mapper.Map<List<OrderDTO>>(orders);
    }

    /// <summary>
    /// Шукає замовлення за номером замовлення та мінімальною загальною вартістю
    /// </summary>
    public async Task<List<OrderDTO>> SearchOrdersAsync(string number, decimal? minTotal)
    {
        var query = _context.Orders
            .Include(o => o.Items)
            .Include(o => o.User)
            .AsNoTracking()
            .Where(o => !o.IsDeleted);

        if (!string.IsNullOrWhiteSpace(number))
        {
            query = query.Where(o => o.OrderNumber.ToLower().Contains(number.Trim().ToLower()));
        }

        var orders = await query.ToListAsync();

        if (minTotal.HasValue)
        {
            orders = orders.Where(o => o.TotalAmount >= minTotal.Value).ToList();
        }

        return _mapper.Map<List<OrderDTO>>(orders);
    }

    /// <summary>
    /// Створює нове замовлення разом із вкладеним переліком товарів та зв'язком із користувачем і товарами
    /// </summary>
    public async Task<OrderDTO> CreateOrderAsync(CreateOrderDTO dto)
    {
        var order = new Order
        {
            UserId = dto.UserId,
            OrderNumber = dto.OrderNumber.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        foreach (var itemDto in dto.Items)
        {
            var item = new OrderItem
            {
                ProductId = itemDto.ProductId,
                ProductName = itemDto.ProductName?.Trim() ?? string.Empty,
                UnitPrice = itemDto.UnitPrice,
                Quantity = itemDto.Quantity
            };

            // Автопідтягування назви та ціни товару з таблиці Products за наявності ProductId
            if (itemDto.ProductId.HasValue && (string.IsNullOrWhiteSpace(item.ProductName) || item.UnitPrice == 0))
            {
                var product = await _context.Products.FindAsync(itemDto.ProductId.Value);
                if (product != null)
                {
                    if (string.IsNullOrWhiteSpace(item.ProductName))
                    {
                        item.ProductName = product.Name;
                    }
                    if (item.UnitPrice == 0)
                    {
                        item.UnitPrice = product.Price;
                    }
                }
            }

            order.Items.Add(item);
        }

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        if (order.UserId.HasValue)
        {
            await _context.Entry(order).Reference(o => o.User).LoadAsync();
        }

        return _mapper.Map<OrderDTO>(order);
    }

    /// <summary>
    /// Виконує м'яке видалення замовлення
    /// </summary>
    public async Task<bool> DeleteOrderAsync(int id)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
        if (order == null) return false;

        order.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Отримує всі окремі позиції замовлень
    /// </summary>
    public async Task<List<OrderItemDTO>> GetAllItemsAsync()
    {
        var items = await _context.OrderItems
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<List<OrderItemDTO>>(items);
    }

    /// <summary>
    /// Знаходить позицію замовлення за її ідентифікатором
    /// </summary>
    public async Task<OrderItemDTO?> GetItemByIdAsync(int id)
    {
        var item = await _context.OrderItems
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id);

        return item == null ? null : _mapper.Map<OrderItemDTO>(item);
    }

    /// <summary>
    /// Додає нову позицію до бази даних
    /// </summary>
    public async Task<OrderItemDTO> CreateItemAsync(CreateOrderItemDTO dto)
    {
        var item = _mapper.Map<OrderItem>(dto);

        // Якщо вказано ProductId і не вказано назву або ціну, беремо з сутності Product
        if (dto.ProductId.HasValue && (string.IsNullOrWhiteSpace(item.ProductName) || item.UnitPrice == 0))
        {
            var product = await _context.Products.FindAsync(dto.ProductId.Value);
            if (product != null)
            {
                if (string.IsNullOrWhiteSpace(item.ProductName))
                {
                    item.ProductName = product.Name;
                }
                if (item.UnitPrice == 0)
                {
                    item.UnitPrice = product.Price;
                }
            }
        }

        await _context.OrderItems.AddAsync(item);
        await _context.SaveChangesAsync();

        return _mapper.Map<OrderItemDTO>(item);
    }

    /// <summary>
    /// Оновлює дані наявної позиції замовлення
    /// </summary>
    public async Task<OrderItemDTO?> UpdateItemAsync(int id, UpdateOrderItemDTO dto)
    {
        var item = await _context.OrderItems.FindAsync(id);
        if (item == null) return null;

        item.ProductId = dto.ProductId;
        item.ProductName = dto.ProductName.Trim();
        item.UnitPrice = dto.UnitPrice;
        item.Quantity = dto.Quantity;

        await _context.SaveChangesAsync();
        return _mapper.Map<OrderItemDTO>(item);
    }

    /// <summary>
    /// Видаляє позицію замовлення з бази даних
    /// </summary>
    public async Task<bool> DeleteItemAsync(int id)
    {
        var item = await _context.OrderItems.FindAsync(id);
        if (item == null) return false;

        _context.OrderItems.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }

    #region Alternative In-Memory Storage
    /*
    // Альтернативне in-memory сховище замовлень без використання бази даних
    private static readonly List<Order> _inMemoryOrders = new()
    {
        new Order
        {
            Id = 1,
            OrderNumber = "ORD_2026_001",
            CreatedAt = DateTime.UtcNow.AddDays(-2),
            Items = new List<OrderItem>
            {
                new OrderItem { Id = 1, ProductId = 1, ProductName = "Laptop ASUS ZenBook", UnitPrice = 38999m, Quantity = 1 }
            }
        }
    };
    */
    #endregion
}
