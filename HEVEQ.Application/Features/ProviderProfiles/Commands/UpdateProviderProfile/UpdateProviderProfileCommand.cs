using MediatR;
using HEVEQ.Application.Features.ProviderProfiles.DTOs;

namespace HEVEQ.Application.Features.ProviderProfiles.Commands.UpdateProviderProfile;

public record UpdateProviderProfileCommand(
    // ApplicationUser fields
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string? PhoneNumber,

    // ProviderProfile fields
    string CompanyName,
    string? BusinessDescription,
    decimal? BaseLatitude,
    decimal? BaseLongitude,
    int ServiceRadiusKm
) : IRequest<ProviderProfileDto>;