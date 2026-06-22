using HEVEQ.Application.Common.Interfaces;

using HEVEQ.Application.Features.ServiceListings.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HEVEQ.Application.Features.ServiceListings.Queries;

public record GetApprovedServiceListingsQuery : IRequest<List<ServiceListingDto>>;

public class GetApprovedServiceListingsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetApprovedServiceListingsQuery, List<ServiceListingDto>>
{
    public async Task<List<ServiceListingDto>> Handle(
        GetApprovedServiceListingsQuery request, CancellationToken cancellationToken)
    {
        return await context.ServiceListings
            .AsNoTracking()
           .Where(l => l.Status == ServiceListingStatus.Active
             && l.ServiceListingOperators.Any())
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