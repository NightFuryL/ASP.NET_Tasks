using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ExtraAspNetAssignments.Data;
using ExtraAspNetAssignments.DTOs.Project;
using ExtraAspNetAssignments.Models.Project;
using ExtraAspNetAssignments.Services.Abstract;

namespace ExtraAspNetAssignments.Services;

/// <summary>
/// Сервіс для управління проєктами та їхнім списком задач через Entity Framework Core
/// </summary>
public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ProjectService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Отримує всі активні проєкти з підвантаженими задачами
    /// </summary>
    public async Task<IEnumerable<ProjectDto>> GetAllAsync()
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    /// <summary>
    /// Знаходить проєкт за його ідентифікатором
    /// </summary>
    public async Task<ProjectDto?> GetByIdAsync(Guid id)
    {
        var project = await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        return project == null ? null : _mapper.Map<ProjectDto>(project);
    }

    /// <summary>
    /// Створює новий проєкт та додає його до бази даних
    /// </summary>
    public async Task<ProjectDto> CreateAsync(CreateProjectDto dto)
    {
        var project = _mapper.Map<Project>(dto);
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();

        return _mapper.Map<ProjectDto>(project);
    }

    /// <summary>
    /// Оновлює назву та опис наявного проєкту
    /// </summary>
    public async Task<bool> UpdateAsync(Guid id, UpdateProjectDto dto)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        if (project == null) return false;

        _mapper.Map(dto, project);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Виконує м'яке видалення проєкту
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        if (project == null) return false;

        project.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }
}
