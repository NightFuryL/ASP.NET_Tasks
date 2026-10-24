using AutoMapper;
using AspNetAssignments.Models.Product;
using AspNetAssignments.Models.User;
using AspNetAssignments.Models.Order;
using AspNetAssignments.DTOs.Product;
using AspNetAssignments.DTOs.User;
using AspNetAssignments.DTOs.Order;

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
        CreateMap<Product, ProductDTO>();
        CreateMap<CreateProductDTO, Product>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));
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
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));
        CreateMap<CreateOrderDTO, Order>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));

        CreateMap<OrderItem, OrderItemDTO>()
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total));
        CreateMap<CreateOrderItemDTO, OrderItem>();
        CreateMap<UpdateOrderItemDTO, OrderItem>();
    }
}
