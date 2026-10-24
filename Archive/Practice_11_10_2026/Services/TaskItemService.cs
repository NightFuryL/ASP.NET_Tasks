using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Practice_11_10_2026.Data;
using Practice_11_10_2026.DTOs;
using Practice_11_10_2026.Models;

namespace Practice_11_10_2026.Services;

public class TaskItemService : ITaskItemService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TaskItemService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TaskItemDto>> GetAllAsync()
    {
        return await _context.TaskItems
            .ProjectTo<TaskItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<TaskItemDto?> GetByIdAsync(Guid id)
    {
        var task = await _context.TaskItems.FindAsync(id);
        return task == null ? null : _mapper.Map<TaskItemDto>(task);
    }

    public async Task<TaskItemDto> CreateAsync(CreateTaskItemDto dto)
    {
        var task = _mapper.Map<TaskItem>(dto);
        _context.TaskItems.Add(task);
        await _context.SaveChangesAsync();
        return _mapper.Map<TaskItemDto>(task);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateTaskItemDto dto)
    {
        var task = await _context.TaskItems.FindAsync(id);
        if (task == null) return false;

        _mapper.Map(dto, task);
        _context.TaskItems.Update(task);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var task = await _context.TaskItems.FindAsync(id);
        if (task == null) return false;

        task.IsDeleted = true;
        _context.TaskItems.Update(task);
        await _context.SaveChangesAsync();
        return true;
    }
}