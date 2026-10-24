using AspNetAssignments.DTOs.Order;

namespace AspNetAssignments.Services.Abstract;

/// <summary>
/// Контракт сервісу для управління замовленнями та їх позиціями
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Отримує всі активні замовлення
    /// </summary>
    Task<List<OrderDTO>> GetAllOrdersAsync();

    /// <summary>
    /// Знаходить замовлення за його ідентифікатором
    /// </summary>
    Task<OrderDTO?> GetOrderByIdAsync(int id);

    /// <summary>
    /// Шукає замовлення за номером та мінімальною сумою
    /// </summary>
    Task<List<OrderDTO>> SearchOrdersAsync(string number, decimal? minTotal);

    /// <summary>
    /// Створює нове замовлення разом із вкладеними позиціями
    /// </summary>
    Task<OrderDTO> CreateOrderAsync(CreateOrderDTO dto);

    /// <summary>
    /// Виконує видалення замовлення
    /// </summary>
    Task<bool> DeleteOrderAsync(int id);

    /// <summary>
    /// Отримує список усіх позицій замовлень
    /// </summary>
    Task<List<OrderItemDTO>> GetAllItemsAsync();

    /// <summary>
    /// Знаходить позицію замовлення за ідентифікатором
    /// </summary>
    Task<OrderItemDTO?> GetItemByIdAsync(int id);

    /// <summary>
    /// Створює нову окрему позицію замовлення
    /// </summary>
    Task<OrderItemDTO> CreateItemAsync(CreateOrderItemDTO dto);

    /// <summary>
    /// Оновлює дані позиції замовлення
    /// </summary>
    Task<OrderItemDTO?> UpdateItemAsync(int id, UpdateOrderItemDTO dto);

    /// <summary>
    /// Видаляє позицію замовлення
    /// </summary>
    Task<bool> DeleteItemAsync(int id);
}
