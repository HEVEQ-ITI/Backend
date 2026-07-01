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
        // ── Address → AddressDto ─────────────────────────────────────────────
        // All properties map by convention (same names, same types).
        // Latitude, Longitude, IsDefault are now included in the DTO.
        CreateMap<Address, AddressDto>();

        // ── CustomerProfile → CustomerProfileDto ─────────────────────────────
        // CustomerProfile fields map automatically by convention.
        // Fields that live on ApplicationUser need explicit ForMember.
        CreateMap<CustomerProfile, CustomerProfileDto>()
            .ForMember(d => d.UserId,
                opt => opt.MapFrom(s => s.User.Id))
            .ForMember(d => d.DisplayName,
                opt => opt.MapFrom(s => s.User.FirstName + " " + s.User.LastName))
            .ForMember(d => d.FirstName,
                opt => opt.MapFrom(s => s.User.FirstName))
            .ForMember(d => d.LastName,
                opt => opt.MapFrom(s => s.User.LastName))
            .ForMember(d => d.Email,
                opt => opt.MapFrom(s => s.User.Email))
            .ForMember(d => d.PhoneNumber,
                opt => opt.MapFrom(s => s.User.PhoneNumber))
            .ForMember(d => d.IsPhoneVerified,
                opt => opt.MapFrom(s => s.User.PhoneNumberConfirmed))
            // DefaultAddress: the single address flagged isDefault=true, or null
            .ForMember(d => d.DefaultAddress,
                opt => opt.MapFrom(s => s.User.Addresses.FirstOrDefault(a => a.IsDefault)));

        // ── CustomerTrustScoreHistory → CustomerTrustHistoryDto ──────────────
        CreateMap<CustomerTrustScoreHistory, CustomerTrustHistoryDto>();

        // ── ProviderProfile → ProviderProfileDto ─────────────────────────────
        // ProviderProfile fields (CompanyName, ServiceRadiusKm, AverageRating,
        // TrustScore, TrustLevel, etc.) map automatically by convention.
        // ApplicationUser fields need explicit mapping.
        CreateMap<ProviderProfile, ProviderProfileDto>()
            .ForMember(d => d.UserId,
                opt => opt.MapFrom(s => s.User.Id))
            .ForMember(d => d.FirstName,
                opt => opt.MapFrom(s => s.User.FirstName))
            .ForMember(d => d.LastName,
                opt => opt.MapFrom(s => s.User.LastName))
            .ForMember(d => d.UserName,
                opt => opt.MapFrom(s => s.User.UserName))
            .ForMember(d => d.Email,
                opt => opt.MapFrom(s => s.User.Email))
            .ForMember(d => d.PhoneNumber,
                opt => opt.MapFrom(s => s.User.PhoneNumber));

        CreateMap<ProviderTrustScoreHistory, ProviderTrustHistoryDto>();

        // ── Operator → OperatorDto ───────────────────────────────────────────
        CreateMap<Operator, OperatorDto>();

        // ── EmployeeProfile → EmployeeProfileDto ─────────────────────────────
        CreateMap<EmployeeProfile, EmployeeProfileDto>()
            .ForMember(d => d.UserId,
                opt => opt.MapFrom(s => s.User.Id))
            .ForMember(d => d.FirstName,
                opt => opt.MapFrom(s => s.User.FirstName))
            .ForMember(d => d.LastName,
                opt => opt.MapFrom(s => s.User.LastName))
            .ForMember(d => d.UserName,
                opt => opt.MapFrom(s => s.User.UserName))
            .ForMember(d => d.Email,
                opt => opt.MapFrom(s => s.User.Email))
            .ForMember(d => d.PhoneNumber,
                opt => opt.MapFrom(s => s.User.PhoneNumber));
    }
}