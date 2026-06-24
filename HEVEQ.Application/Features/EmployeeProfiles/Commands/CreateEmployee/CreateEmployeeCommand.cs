using MediatR;
using HEVEQ.Application.Features.EmployeeProfiles.DTOs;

namespace HEVEQ.Application.Features.EmployeeProfiles.Commands.CreateEmployee;

public record CreateEmployeeCommand(
    // ApplicationUser fields
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Password,
    string? PhoneNumber,

    // EmployeeProfile fields
    string? Department,
    string? AssignedGovernorate,
    bool IsAvailableForDispatch
) : IRequest<EmployeeProfileDto>;