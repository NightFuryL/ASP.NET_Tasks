using AutoMapper;
using Practice_04_10_2026.DTOs;
using Practice_04_10_2026.Models;

namespace Practice_04_10_2026.Profiles;

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
