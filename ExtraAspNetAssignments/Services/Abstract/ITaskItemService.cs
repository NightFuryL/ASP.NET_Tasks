using ExtraAspNetAssignments.DTOs.Project;

namespace ExtraAspNetAssignments.Services.Abstract;

/// <summary>
/// Контракт сервісу для управління окремими задачами всередині проєктів
/// </summary>
public interface ITaskItemService
{
    /// <summary>
    /// Отримує всі активні задачі
    /// </summary>
    Task<IEnumerable<TaskItemDto>> GetAllAsync();

    /// <summary>
    /// Знаходить задачу за її ідентифікатором
    /// </summary>
    Task<TaskItemDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Створює нову задачу та прив'язує її до проєкту
    /// </summary>
    Task<TaskItemDto> CreateAsync(CreateTaskItemDto dto);

    /// <summary>
    /// Оновлює статус, пріоритет або опис задачі
    /// </summary>
    Task<bool> UpdateAsync(Guid id, UpdateTaskItemDto dto);

    /// <summary>
    /// Виконує м'яке видалення задачі
    /// </summary>
    Task<bool> DeleteAsync(Guid id);
}
