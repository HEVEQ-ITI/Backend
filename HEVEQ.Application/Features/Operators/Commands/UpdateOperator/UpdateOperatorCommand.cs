using MediatR;
using HEVEQ.Application.Features.Operators.DTOs;

namespace HEVEQ.Application.Features.Operators.Commands.UpdateOperator;

public record UpdateOperatorCommand(
    Guid Id,       // operator id from route
    string FullName,
    int? YearsOfExperience,
    string? Specialization,
    string? LicenseType,
    string? LicenseNumber,
    DateOnly? LicenseExpiryDate,
    string? ProfilePhotoUrl,
    bool IsActive
) : IRequest<OperatorDto>;