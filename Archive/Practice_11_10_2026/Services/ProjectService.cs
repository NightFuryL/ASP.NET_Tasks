using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Practice_11_10_2026.Data;
using Practice_11_10_2026.DTOs;
using Practice_11_10_2026.Models;

namespace Practice_11_10_2026.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ProjectService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllAsync()
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<ProjectDto?> GetByIdAsync(Guid id)
    {
        var project = await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id);

        return project == null ? null : _mapper.Map<ProjectDto>(project);
    }

    public async Task<ProjectDto> CreateAsync(CreateProjectDto dto)
    {
        var project = _mapper.Map<Project>(dto);
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateProjectDto dto)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null) return false;

        _mapper.Map(dto, project);
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null) return false;

        project.IsDeleted = true;
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
        return true;
    }
}