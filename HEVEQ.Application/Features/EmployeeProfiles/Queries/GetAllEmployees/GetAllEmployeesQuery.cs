using MediatR;
using HEVEQ.Application.Features.EmployeeProfiles.DTOs;

namespace HEVEQ.Application.Features.EmployeeProfiles.Queries.GetAllEmployees;

public record GetAllEmployeesQuery : IRequest<List<EmployeeProfileDto>>;