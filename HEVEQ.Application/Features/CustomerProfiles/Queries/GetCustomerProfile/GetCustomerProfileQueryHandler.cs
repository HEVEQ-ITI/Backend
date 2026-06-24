using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.CustomerProfiles.DTOs;
using HEVEQ.Domain.Entities;

namespace HEVEQ.Application.Features.CustomerProfiles.Queries.GetCustomerProfile;

public class GetCustomerProfileQueryHandler
    : IRequestHandler<GetCustomerProfileQuery, CustomerProfileDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetCustomerProfileQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<CustomerProfileDto> Handle(
        GetCustomerProfileQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        var dto = await _context.CustomerProfiles
            .Include(p => p.User)
                .ThenInclude(u => u.Addresses)
            .Where(p => p.UserId == userId)
            .ProjectTo<CustomerProfileDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return dto ?? throw new NotFoundException(nameof(CustomerProfile), userId);
    }
}