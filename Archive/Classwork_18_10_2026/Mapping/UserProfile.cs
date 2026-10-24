using AutoMapper;
using Classwork_18_10_2026.Data.Models;
using Classwork_18_10_2026.DTO;

namespace Classwork_18_10_2026.Mapping;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.Name} {src.LastName}"));
    }
}
