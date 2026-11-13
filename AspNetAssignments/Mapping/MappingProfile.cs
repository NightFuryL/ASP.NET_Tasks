using AspNetAssignments.DTOs.Order;
using AspNetAssignments.DTOs.Product;
using AspNetAssignments.DTOs.User;
using AspNetAssignments.Models.Order;
using AspNetAssignments.Models.Product;
using AspNetAssignments.Models.User;
using AutoMapper;
#region Додаткові сутності
using AspNetAssignments.DTOs.Book;
using AspNetAssignments.DTOs.Invoice;
using AspNetAssignments.DTOs.Project;
using AspNetAssignments.Models.Book;
using AspNetAssignments.Models.Invoice;
using AspNetAssignments.Models.Project;
#endregion

namespace AspNetAssignments.Mapping;

/// <summary>
/// Профіль AutoMapper для перетворення сутностей у DTO та навпаки
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Ініціалізація правил мапінгу для сутностей системи
    /// </summary>
    public MappingProfile()
    {
        // Мапінг для сутності Product
        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => i.ImageUrl).ToList()));
        CreateMap<CreateProductDTO, Product>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.Images, opt => opt.Ignore());
        CreateMap<UpdateProductDTO, Product>();

        // Мапінг для сутності User
        CreateMap<User, UserDTO>();
        CreateMap<CreateUserDTO, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.AvatarURL, opt => opt.Ignore());
        CreateMap<UpdateUserDTO, User>()
            .ForMember(dest => dest.AvatarURL, opt => opt.Ignore());

        // Мапінг для замовлень та позицій
        CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? $"{src.User.Name} {src.User.LastName}".Trim() : null))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));
        CreateMap<CreateOrderDTO, Order>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));

        CreateMap<OrderItem, OrderItemDTO>()
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total));
        CreateMap<CreateOrderItemDTO, OrderItem>();
        CreateMap<UpdateOrderItemDTO, OrderItem>();

        #region Додаткові сутності
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
        #endregion
    }
}
