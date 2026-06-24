using MediatR;
using HEVEQ.Application.Features.EmployeeProfiles.DTOs;

namespace HEVEQ.Application.Features.EmployeeProfiles.Commands.UpdateEmployee;

public record UpdateEmployeeCommand(
    // Set from route by controller, not from request body
    Guid EmployeeProfileId,

    // ApplicationUser fields admin can update
    string FirstName,
    string LastName,
    string? PhoneNumber,

    // EmployeeProfile fields admin can update
    string? Department,
    string? AssignedGovernorate,
    bool IsAvailableForDispatch
) : IRequest<EmployeeProfileDto>;