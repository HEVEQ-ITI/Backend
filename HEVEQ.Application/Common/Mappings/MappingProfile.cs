using AutoMapper;
using HEVEQ.Application.Features.CustomerProfiles.DTOs;
using HEVEQ.Application.Features.EmployeeProfiles.DTOs;
using HEVEQ.Application.Features.Operators.DTOs;
using HEVEQ.Application.Features.ProviderProfiles.DTOs;
using HEVEQ.Domain.Entities;

namespace HEVEQ.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Address → AddressDto (simple, all names match)
        CreateMap<Address, AddressDto>();

        // CustomerProfile → CustomerProfileDto
        // Fields from CustomerProfile map automatically (same names)
        // Fields from User are pulled manually via ForMember
        CreateMap<CustomerProfile, CustomerProfileDto>()
            .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.User.Id))
            .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.User.FirstName))
            .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.User.LastName))
            .ForMember(d => d.Email, opt => opt.MapFrom(s => s.User.Email))
            .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(s => s.User.PhoneNumber))
            .ForMember(d => d.DefaultAddress,
                opt => opt.MapFrom(s => s.User.Addresses.FirstOrDefault(a => a.IsDefault)));

        // CustomerTrustScoreHistory → CustomerTrustHistoryDto (all names match)
        CreateMap<CustomerTrustScoreHistory, CustomerTrustHistoryDto>();

        // ── Provider Profile ──────────────────────────────────────────────────
        CreateMap<ProviderProfile, ProviderProfileDto>()
            .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.User.Id))
            .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.User.FirstName))
            .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.User.LastName))
            .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.UserName))
            .ForMember(d => d.Email, opt => opt.MapFrom(s => s.User.Email))
            .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(s => s.User.PhoneNumber));

        CreateMap<ProviderTrustScoreHistory, ProviderTrustHistoryDto>();

        // ── Operator ──────────────────────────────────────────────────────────
        CreateMap<Operator, OperatorDto>();

        CreateMap<EmployeeProfile, EmployeeProfileDto>()
    .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.User.Id))
    .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.User.FirstName))
    .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.User.LastName))
    .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.UserName))
    .ForMember(d => d.Email, opt => opt.MapFrom(s => s.User.Email))
    .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(s => s.User.PhoneNumber));

    }
}   