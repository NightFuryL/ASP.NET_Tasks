using Practice_17_10_2026.DTO.Order;
using Practice_17_10_2026.DTO.Pagination;

namespace Practice_17_10_2026.Services;

public interface IOrderService
{
    Task<PageResponse<OrderDTO>> GetPagedAsync(OrderQueryParams queryParams);
    Task<OrderDTO?> GetOrderByIdAsync(Guid id);
    Task<OrderDTO> CreateOrderAsync(CreateOrderDTO dto);
    Task<bool> DeleteAsync(Guid id);
}
