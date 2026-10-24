using AutoMapper;
using ExtraAspNetAssignments.Models.Book;
using ExtraAspNetAssignments.Models.Invoice;
using ExtraAspNetAssignments.Models.Project;
using ExtraAspNetAssignments.DTOs.Book;
using ExtraAspNetAssignments.DTOs.Invoice;
using ExtraAspNetAssignments.DTOs.Project;

namespace ExtraAspNetAssignments.Mapping;

/// <summary>
/// Профіль AutoMapper для мапінгу проєктів, задач, інвойсів та книг
/// </summary>
public class ExtraMappingProfile : Profile
{
    /// <summary>
    /// Ініціалізація правил мапінгу додаткових сутностей
    /// </summary>
    public ExtraMappingProfile()
    {
        // Мапінг для Project та TaskItem
        CreateMap<Project, ProjectDto>().ReverseMap();
        CreateMap<CreateProjectDto, Project>();
        CreateMap<UpdateProjectDto, Project>();

        CreateMap<TaskItem, TaskItemDto>().ReverseMap();
        CreateMap<CreateTaskItemDto, TaskItem>();
        CreateMap<UpdateTaskItemDto, TaskItem>();

        // Мапінг для Invoice
        CreateMap<Invoice, InvoiceReadDto>();
        CreateMap<InvoiceCreateDto, Invoice>();
        CreateMap<InvoiceUpdateDto, Invoice>();

        // Мапінг для Book
        CreateMap<Book, BookDto>().ReverseMap();
        CreateMap<CreateBookDto, Book>();
        CreateMap<UpdateBookDto, Book>();
    }
}
