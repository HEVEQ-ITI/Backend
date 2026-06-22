using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.ServiceListings.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HEVEQ.Application.Features.ServiceListings.Queries;

public record GetProviderServiceListingsQuery : IRequest<List<ServiceListingDto>>;

public class GetProviderServiceListingsQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetProviderServiceListingsQuery, List<ServiceListingDto>>
{
    public async Task<List<ServiceListingDto>> Handle(
        GetProviderServiceListingsQuery request, CancellationToken cancellationToken)
    {
     
        if (!currentUserService.UserId.HasValue)
            throw new ForbiddenAccessException("User is not authenticated.");

        var userId = currentUserService.UserId.Value;

        var providerProfileId = await context.ProviderProfiles
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .Select(p => p.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (providerProfileId == Guid.Empty)
            throw new ForbiddenAccessException("Provider profile not found for current user.");

        return await context.ServiceListings
            .AsNoTracking()
            .Where(l => l.ProviderProfileId == providerProfileId)
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new ServiceListingDto
            {
                Id = l.Id,
                ProviderProfileId = l.ProviderProfileId,
                CategoryId = l.CategoryId,
                Title = l.Title,
                Tags = l.Tags,
                EquipmentModel = l.EquipmentModel,
                HourlyRate = l.HourlyRate,
                DailyRate = l.DailyRate,
                MinimumBookingHours = l.MinimumBookingHours,
                Status = l.Status,
                QualityScore = l.QualityScore,
                CreatedAt = l.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}