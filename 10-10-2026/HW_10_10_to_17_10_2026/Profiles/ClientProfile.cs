using AutoMapper;
using HW_10_10_to_17_10_2026.DTOs;
using HW_10_10_to_17_10_2026.Models;

namespace HW_10_10_to_17_10_2026.Profiles;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<Client, ClientReadDto>();
        CreateMap<ClientReadDto, Client>();

        CreateMap<ClientCreateDto, Client>();
        CreateMap<Client, ClientCreateDto>();
    }
}
