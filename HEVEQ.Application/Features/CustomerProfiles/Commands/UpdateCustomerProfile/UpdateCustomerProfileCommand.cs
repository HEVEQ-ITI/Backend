using HEVEQ.Application.Features.CustomerProfiles.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.CustomerProfiles.Commands.UpdateCustomerProfile;

public record UpdateCustomerProfileCommand(
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    UpdateAddressCommand? DefaultAddress
) : IRequest<CustomerProfileDto>;

// Nested record — keeps address fields grouped cleanly inside the same command
public record UpdateAddressCommand(
    string? Label,
    string Governorate,
    string District,
    string? Street
   
);