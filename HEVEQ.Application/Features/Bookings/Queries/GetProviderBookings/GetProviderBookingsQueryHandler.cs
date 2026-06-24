using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HEVEQ.Application.Features.Bookings.Queries.GetProviderBookings
{
    public sealed class GetProviderBookingsQueryHandler : IRequestHandler<GetProviderBookingsQuery, IReadOnlyList<ProviderBookingListItemDto>>
    {
        private readonly IApplicationDbContext _context;
        public GetProviderBookingsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ProviderBookingListItemDto>> Handle(GetProviderBookingsQuery request, CancellationToken cancellationToken)
        {
            var providerProfileId = await _context.ProviderProfiles
                .AsNoTracking()
                .Where(x => x.UserId == request.ProviderUserId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (providerProfileId == Guid.Empty)
                throw new InvalidOperationException("Provider profile was not found.");

            var bookings = await _context.Bookings
                .Include(x => x.ServiceListing)
                .Include(x => x.Customer)
                .AsNoTracking()
                .Where(x => x.ServiceListing.ProviderProfileId == providerProfileId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new
                {
                    x.Id,
                    CustomerName = x.Customer.FirstName + " " + x.Customer.LastName,
                    ServiceListingTitle = x.ServiceListing.Title,
                    x.RequestedStartDate,
                    x.RequestedStartTime,
                    x.EstimatedDurationHours,
                    x.EstimatedTotal,
                    x.Status
                })
                .ToListAsync(cancellationToken);

            return bookings.Select(x => new ProviderBookingListItemDto
            {
                BookingId = x.Id,
                BookingNumber = "TEST", // Unique number will be created for display
                CustomerName = x.CustomerName.Trim(),
                ServiceListingTitle = x.ServiceListingTitle,
                RequestedStartDate = x.RequestedStartDate,
                RequestedStartTime = x.RequestedStartTime,
                EstimatedDurationHours = x.EstimatedDurationHours,
                EstimatedTotal = x.EstimatedTotal,
                Status = x.Status.ToString(),
                CanAccept = x.Status == BookingStatus.PendingProviderResponse,
                CanReject = x.Status == BookingStatus.PendingProviderResponse
            }).ToList();
        }
    }
}