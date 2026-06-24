using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Application.Features.Bookings.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HEVEQ.Application.Features.Bookings.Queries.GetCustomerBookings
{
    public sealed class GetCustomerBookingsQueryHandler
        : IRequestHandler<GetCustomerBookingsQuery, IReadOnlyList<CustomerBookingListItemDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetCustomerBookingsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<CustomerBookingListItemDto>> Handle(GetCustomerBookingsQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _context.Bookings
                .Include(x => x.ServiceListing)
                    .ThenInclude(x => x.ProviderProfile)
                .AsNoTracking()
                .Where(x => x.CustomerId == request.CustomerId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new
                {
                    x.Id,
                    ServiceListingTitle = x.ServiceListing.Title,
                    ProviderName = x.ServiceListing.ProviderProfile.CompanyName,
                    x.RequestedStartDate,
                    x.RequestedStartTime,
                    x.EstimatedDurationHours,
                    x.EstimatedTotal,
                    x.Status
                })
                .ToListAsync(cancellationToken);

            return bookings.Select(x => new CustomerBookingListItemDto
            {
                BookingId = x.Id,
                BookingNumber = "TEST",//Unique number will be creted for display
                ServiceListingTitle = x.ServiceListingTitle,
                ProviderName = x.ProviderName,
                RequestedStartDate = x.RequestedStartDate,
                RequestedStartTime = x.RequestedStartTime,
                EstimatedDurationHours = x.EstimatedDurationHours,
                EstimatedTotal = x.EstimatedTotal,
                Status = x.Status.ToString()
            }).ToList();
        }
    }
}