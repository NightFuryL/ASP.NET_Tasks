using AutoMapper;
using Classwork_04_10_2026.DTO;
using Classwork_04_10_2026.DataBase;
namespace Classwork_04_10_2026.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateProductDTO, Product>();
        CreateMap<Product, Product>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => false));
    }
}
