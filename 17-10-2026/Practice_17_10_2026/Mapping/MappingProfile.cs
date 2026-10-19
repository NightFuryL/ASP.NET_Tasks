using AutoMapper;
using Practice_17_10_2026.Data.Models;
using Practice_17_10_2026.DTO.Order;

namespace Practice_17_10_2026.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Order, OrderDTO>();
        CreateMap<CreateOrderDTO, Order>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));
    }
}
