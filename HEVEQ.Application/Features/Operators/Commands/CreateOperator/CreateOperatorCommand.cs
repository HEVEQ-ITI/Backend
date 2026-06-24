using MediatR;
using HEVEQ.Application.Features.Operators.DTOs;

namespace HEVEQ.Application.Features.Operators.Commands.CreateOperator;

public record CreateOperatorCommand(
    string FullName,
    int? YearsOfExperience,
    string? Specialization,
    string? LicenseType,
    string? LicenseNumber,
    DateOnly? LicenseExpiryDate,
    string? ProfilePhotoUrl
) : IRequest<OperatorDto>;