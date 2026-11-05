using AspNetAssignments.DTOs.Project;

namespace AspNetAssignments.Services.Abstract;

/// <summary>
/// Контракт сервісу для управління проєктами
/// </summary>
public interface IProjectService
{
    /// <summary>
    /// Отримує перелік усіх проєктів разом із задачами
    /// </summary>
    Task<IEnumerable<ProjectDto>> GetAllAsync();

    /// <summary>
    /// Знаходить проєкт за його унікальним ідентифікатором
    /// </summary>
    Task<ProjectDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Створює новий проєкт
    /// </summary>
    Task<ProjectDto> CreateAsync(CreateProjectDto dto);

    /// <summary>
    /// Оновлює дані наявного проєкту
    /// </summary>
    Task<bool> UpdateAsync(Guid id, UpdateProjectDto dto);

    /// <summary>
    /// Виконує м'яке видалення проєкту
    /// </summary>
    Task<bool> DeleteAsync(Guid id);
}
