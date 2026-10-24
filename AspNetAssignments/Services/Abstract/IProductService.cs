using AspNetAssignments.DTOs.Pagination;
using AspNetAssignments.DTOs.Product;

namespace AspNetAssignments.Services.Abstract;

/// <summary>
/// Контракт сервісу для управління каталогом товарів
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Отримує сторінку товарів із фільтрацією та сортуванням
    /// </summary>
    Task<PageResponse<ProductDTO>> GetPagedAsync(ProductQueryParams queryParams);

    /// <summary>
    /// Отримує повний перелік усіх активних товарів
    /// </summary>
    Task<List<ProductDTO>> GetAllProductsAsync();

    /// <summary>
    /// Знаходить товар за його унікальним ідентифікатором
    /// </summary>
    Task<ProductDTO?> GetProductByIdAsync(Guid id);

    /// <summary>
    /// Створює новий товар у системі
    /// </summary>
    Task<ProductDTO> CreateProductAsync(CreateProductDTO dto);

    /// <summary>
    /// Оновлює дані наявного товару
    /// </summary>
    Task<ProductDTO?> UpdateProductAsync(Guid id, UpdateProductDTO dto);

    /// <summary>
    /// Виконує м'яке видалення товару за ідентифікатором
    /// </summary>
    Task<bool> DeleteAsync(Guid id);
}
