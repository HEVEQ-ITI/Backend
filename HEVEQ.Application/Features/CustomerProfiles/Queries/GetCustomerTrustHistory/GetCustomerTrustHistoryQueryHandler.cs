using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.CustomerProfiles.DTOs;
using HEVEQ.Domain.Entities;

namespace HEVEQ.Application.Features.CustomerProfiles.Queries.GetCustomerTrustHistory;

public class GetCustomerTrustHistoryQueryHandler
    : IRequestHandler<GetCustomerTrustHistoryQuery, List<CustomerTrustHistoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetCustomerTrustHistoryQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<List<CustomerTrustHistoryDto>> Handle(
        GetCustomerTrustHistoryQuery request,
        CancellationToken cancellationToken)
    {
        // ── Step 1: get current user ──────────────────────────────────
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        // ── Step 2: find their CustomerProfile to get its Id ──────────
        // We need CustomerProfileId (not UserId) to query the history table
        var profileId = await _context.CustomerProfiles
            .Where(p => p.UserId == userId)
            .Select(p => p.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (profileId == Guid.Empty)
            throw new NotFoundException(nameof(CustomerProfile), userId);

        // ── Step 3: return history ordered newest first ────────────────
        return await _context.CustomerTrustScoreHistory
            .Where(h => h.CustomerProfileId == profileId)
            .OrderByDescending(h => h.RecordedAt)
            .ProjectTo<CustomerTrustHistoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
