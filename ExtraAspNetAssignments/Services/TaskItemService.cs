using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ExtraAspNetAssignments.Data;
using ExtraAspNetAssignments.DTOs.Project;
using ExtraAspNetAssignments.Models.Project;
using ExtraAspNetAssignments.Services.Abstract;

namespace ExtraAspNetAssignments.Services;

/// <summary>
/// Сервіс для управління задачами в межах проєктів через Entity Framework Core
/// </summary>
public class TaskItemService : ITaskItemService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TaskItemService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Отримує перелік усіх активних задач
    /// </summary>
    public async Task<IEnumerable<TaskItemDto>> GetAllAsync()
    {
        return await _context.TaskItems
            .AsNoTracking()
            .Where(t => !t.IsDeleted)
            .ProjectTo<TaskItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    /// <summary>
    /// Знаходить задачу за її ідентифікатором
    /// </summary>
    public async Task<TaskItemDto?> GetByIdAsync(Guid id)
    {
        var task = await _context.TaskItems
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

        return task == null ? null : _mapper.Map<TaskItemDto>(task);
    }

    /// <summary>
    /// Створює нову задачу та прив'язує її до проєкту
    /// </summary>
    public async Task<TaskItemDto> CreateAsync(CreateTaskItemDto dto)
    {
        var task = _mapper.Map<TaskItem>(dto);
        await _context.TaskItems.AddAsync(task);
        await _context.SaveChangesAsync();

        return _mapper.Map<TaskItemDto>(task);
    }

    /// <summary>
    /// Оновлює інформацію про задачу (статус, пріоритет, опис)
    /// </summary>
    public async Task<bool> UpdateAsync(Guid id, UpdateTaskItemDto dto)
    {
        var task = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        if (task == null) return false;

        _mapper.Map(dto, task);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Виконує м'яке видалення задачі
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id)
    {
        var task = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        if (task == null) return false;

        task.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }
}
