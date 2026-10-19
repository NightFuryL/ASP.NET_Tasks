using AutoMapper;
using HW_17_10_to_24_10_2026.DTO;
using HW_17_10_to_24_10_2026.Models;

namespace HW_17_10_to_24_10_2026.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));
    }
}
