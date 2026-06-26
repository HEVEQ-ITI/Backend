using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.ServiceListings.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HEVEQ.Application.Features.ServiceListings.Queries.GetPublicServiceListingById;

public class GetPublicServiceListingByIdQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetPublicServiceListingByIdQuery, PublicServiceListingDetailDto?>
{
    public async Task<PublicServiceListingDetailDto?> Handle(
        GetPublicServiceListingByIdQuery request, CancellationToken cancellationToken)
    {
        // 1. fetch listing with essential includes and apply active filter for guests/customers
        var listing = await context.ServiceListings
            .AsNoTracking()
            .Include(l => l.Category)
            .Include(l => l.ProviderProfile)
            .Include(l => l.Photos)
            .Include(l => l.ServiceListingOperators)
                .ThenInclude(lo => lo.Operator)
            .FirstOrDefaultAsync(l => l.Id == request.Id && l.Status == ServiceListingStatus.Active, cancellationToken);

        // 2. return null if listing not found or not active
        if (listing == null)
        {
            return null;
        }

        // 3. map photos list safely
        var photos = listing.Photos != null
            ? listing.Photos.OrderBy(p => p.DisplayOrder).Select(p => p.PhotoUrl).ToList()
            : new List<string>();

        // 4. map operators list safely using fallback for null entries
        var operators = listing.ServiceListingOperators != null
            ? listing.ServiceListingOperators
                .Where(lo => lo.Operator != null)
                .Select(lo => new PublicOperatorSummaryDto(
                    lo.Operator!.Id,
                    lo.Operator.FullName ?? "Unknown Operator",
                    lo.Operator.LicenseType,
                    4.5 // Temporary dummy rating for UI until rating system is linked
                )).ToList()
            : new List<PublicOperatorSummaryDto>();

        // 5. map provider summary details with fallbacks
        var providerDto = new PublicProviderSummaryDto(
            listing.ProviderProfile != null ? listing.ProviderProfile.CompanyName : "Unknown Provider",
            listing.ProviderProfile != null ? (double)listing.ProviderProfile.AverageRating : 0.0,
            listing.ProviderProfile != null ? listing.ProviderProfile.CompletedBookingsCount : 0,
            listing.ProviderProfile != null ? (int)listing.ProviderProfile.TrustScore : 0,
            listing.ProviderProfile != null ? listing.ProviderProfile.TrustLevel.ToString() : "Standard"
        );

        // 6. map availability list (mocked or mapped based on project stage)
        var availability = new List<PublicAvailabilityDto>();

        // 7. compose final structural public detail dto
        return new PublicServiceListingDetailDto(
            listing.Id,
            listing.Title,
            listing.Description ?? string.Empty,
            listing.Category != null ? listing.Category.Name : "General",
            listing.EquipmentModel,
            listing.EquipmentCapacity,
            listing.EquipmentCondition,
            listing.YearOfManufacture,
            listing.HourlyRate ?? 0,
            listing.DailyRate ?? 0,
            listing.MinimumBookingHours,
            photos,
            availability,
            providerDto,
            operators,
            true // canRequestBooking default flag
        );
    }
}