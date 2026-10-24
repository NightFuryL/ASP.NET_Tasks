using AutoMapper;
using Classwork_10_10_2026.Data.Models;
using Classwork_10_10_2026.DTO;
namespace Classwork_10_10_2026.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>();

            CreateMap<CreateProductDTO, Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));
        }

    }
}
