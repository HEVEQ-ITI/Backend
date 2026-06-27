using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Application.Features.Bookings.Helpers;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HEVEQ.Application.Features.Bookings.Queries.GetProviderBookingRequests
{
    public sealed class GetProviderBookingRequestsQueryHandler : IRequestHandler<GetProviderBookingRequestsQuery, ProviderBookingRequestsResponseDto>
    {
        private readonly IApplicationDbContext _context;
        public GetProviderBookingRequestsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProviderBookingRequestsResponseDto> Handle(GetProviderBookingRequestsQuery request, CancellationToken cancellationToken)
        {
            var providerProfile = await _context.ProviderProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == request.ProviderUserId, cancellationToken);

            if (providerProfile is null)
                throw new InvalidOperationException("Provider profile was not found.");

            var query = _context.Bookings
                .AsNoTracking()
                .Where(x =>
                    x.ServiceListing.ProviderProfileId == providerProfile.Id &&
                    x.Status == BookingStatus.PendingProviderResponse);

            var totalCount = await query.CountAsync(cancellationToken);

            var bookings = await query
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new
                {
                    x.Id,
                    x.BookingNumber,
                    CustomerName = x.Customer.FirstName + " " + x.Customer.LastName,
                    ServiceTitle = x.ServiceListing.Title,
                    x.JobTitle,
                    x.Governorate,
                    x.District,
                    x.RequestedStartDate,
                    x.RequestedStartTime,
                    x.EstimatedDurationHours,
                    x.EstimatedTotal,
                    x.Status
                })
                .ToListAsync(cancellationToken);

            var items = bookings.Select(x => new ProviderBookingRequestItemDto
            {
                Id = x.Id,
                BookingNumber = x.BookingNumber,
                CustomerName = x.CustomerName.Trim(),
                ServiceTitle = x.ServiceTitle,
                JobTitle = x.JobTitle,
                Location = $"{x.District}, {x.Governorate}",
                RequestedStartDate = x.RequestedStartDate,
                RequestedStartTime = x.RequestedStartTime,
                EstimatedDurationHours = x.EstimatedDurationHours,
                EstimatedTotal = x.EstimatedTotal,
                Status = x.Status.ToString(),
                StatusAr = BookingDisplayHelper.GetStatusAr(x.Status),
                CanAccept = BookingActionsHelper.CanProviderAccept(x.Status),
                CanReject = BookingActionsHelper.CanProviderReject(x.Status)
            }).ToList();

            return new ProviderBookingRequestsResponseDto
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}