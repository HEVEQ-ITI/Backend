using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.EmployeeProfiles.DTOs;

namespace HEVEQ.Application.Features.EmployeeProfiles.Queries.GetAllEmployees;

public class GetAllEmployeesQueryHandler
    : IRequestHandler<GetAllEmployeesQuery, List<EmployeeProfileDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllEmployeesQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<EmployeeProfileDto>> Handle(
        GetAllEmployeesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.EmployeeProfiles
            .Include(e => e.User)
            .OrderBy(e => e.User.FirstName)
            .ProjectTo<EmployeeProfileDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}