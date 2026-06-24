using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.ProviderProfiles.DTOs;
using HEVEQ.Domain.Entities;

namespace HEVEQ.Application.Features.ProviderProfiles.Queries.GetProviderTrustHistory;

public class GetProviderTrustHistoryQueryHandler
    : IRequestHandler<GetProviderTrustHistoryQuery, List<ProviderTrustHistoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetProviderTrustHistoryQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<List<ProviderTrustHistoryDto>> Handle(
        GetProviderTrustHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        // Step 1: find this provider's profile id
        var profileId = await _context.ProviderProfiles
            .Where(p => p.UserId == userId)
            .Select(p => p.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (profileId == Guid.Empty)
            throw new NotFoundException(nameof(ProviderProfile), userId);

        // Step 2: fetch history for that profile only
        return await _context.ProviderTrustScoreHistory
            .Where(h => h.ProviderProfileId == profileId)
            .OrderByDescending(h => h.RecordedAt)
            .ProjectTo<ProviderTrustHistoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}