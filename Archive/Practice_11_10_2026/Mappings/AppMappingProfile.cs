using AutoMapper;
using Practice_11_10_2026.DTOs;
using Practice_11_10_2026.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Practice_11_10_2026.Mappings;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<Project, ProjectDto>().ReverseMap();
        CreateMap<CreateProjectDto, Project>();
        CreateMap<UpdateProjectDto, Project>();

        CreateMap<TaskItem, TaskItemDto>().ReverseMap();
        CreateMap<CreateTaskItemDto, TaskItem>();
        CreateMap<UpdateTaskItemDto, TaskItem>();
    }
}