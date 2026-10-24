using AutoMapper;
using HW_04_10_to_12_10_2026.DTOs;
using HW_04_10_to_12_10_2026.Models;

namespace HW_04_10_to_12_10_2026.Profiles;

public class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        CreateMap<Invoice, InvoiceReadDto>();
        CreateMap<InvoiceReadDto, Invoice>();

        CreateMap<InvoiceCreateDto, Invoice>();
        CreateMap<Invoice, InvoiceCreateDto>();

        CreateMap<InvoiceUpdateDto, Invoice>();
        CreateMap<Invoice, InvoiceUpdateDto>();
    }
}
