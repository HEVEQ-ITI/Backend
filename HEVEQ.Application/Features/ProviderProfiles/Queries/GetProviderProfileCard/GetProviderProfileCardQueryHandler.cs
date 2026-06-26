using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.ProviderProfiles.DTOs;
using HEVEQ.Domain.Entities;

namespace HEVEQ.Application.Features.ProviderProfiles.Queries.GetProviderProfileCard;

public class GetProviderProfileCardQueryHandler
    : IRequestHandler<GetProviderProfileCardQuery, ProviderProfileCardDto>
{
    private readonly IApplicationDbContext _context;

    public GetProviderProfileCardQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProviderProfileCardDto> Handle(
        GetProviderProfileCardQuery request,
        CancellationToken cancellationToken)
    {
        var card = await _context.ProviderProfiles
            .Where(p => p.Id == request.ProviderProfileId)
            .Select(p => new ProviderProfileCardDto
            {
                ProviderProfileId = p.Id,
                CompanyName = p.CompanyName,
                AverageRating = p.AverageRating,
                TotalReviewsCount = p.TotalReviewsCount,
                CompletedBookingsCount = p.CompletedBookingsCount,
                TrustScore = p.TrustScore,
                TrustLevel = p.TrustLevel,
                ActiveSince = p.CreatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        return card ?? throw new NotFoundException(
            nameof(ProviderProfile), request.ProviderProfileId);
    }
}