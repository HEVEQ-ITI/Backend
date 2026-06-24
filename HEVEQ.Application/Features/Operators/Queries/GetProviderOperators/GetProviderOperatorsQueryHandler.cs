using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Operators.DTOs;
using HEVEQ.Domain.Entities;

namespace HEVEQ.Application.Features.Operators.Queries.GetProviderOperators;

public class GetProviderOperatorsQueryHandler
    : IRequestHandler<GetProviderOperatorsQuery, List<OperatorDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetProviderOperatorsQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<List<OperatorDto>> Handle(
        GetProviderOperatorsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        var providerProfileId = await _context.ProviderProfiles
            .Where(p => p.UserId == userId)
            .Select(p => p.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (providerProfileId == Guid.Empty)
            throw new NotFoundException(nameof(ProviderProfile), userId);

        // Only returns operators that belong to this provider — never others
        return await _context.Operators
            .Where(o => o.ProviderProfileId == providerProfileId)
            .OrderBy(o => o.FullName)
            .ProjectTo<OperatorDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}