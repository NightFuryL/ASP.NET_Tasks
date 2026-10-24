using Practice_11_10_2026.DTOs;

namespace Practice_11_10_2026.Services;

public interface ITaskItemService
{
    Task<IEnumerable<TaskItemDto>> GetAllAsync();
    Task<TaskItemDto?> GetByIdAsync(Guid id);
    Task<TaskItemDto> CreateAsync(CreateTaskItemDto dto);
    Task<bool> UpdateAsync(Guid id, UpdateTaskItemDto dto);
    Task<bool> DeleteAsync(Guid id);
}