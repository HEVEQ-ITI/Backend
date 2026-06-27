using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Application.Features.Bookings.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HEVEQ.Application.Features.Bookings.Queries.GetCustomerBookings
{
    public sealed class GetCustomerBookingsQueryHandler : IRequestHandler<GetCustomerBookingsQuery, CustomerBookingsResponseDto>
    {
        private readonly IApplicationDbContext _context;
        public GetCustomerBookingsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerBookingsResponseDto> Handle(GetCustomerBookingsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Bookings
                .AsNoTracking()
                .Where(x => x.CustomerId == request.CustomerId);

            if (request.Status.HasValue)
            {
                query = query.Where(x => x.Status == request.Status.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var bookings = await query
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new
                {
                    x.Id,
                    x.BookingNumber,
                    ServiceTitle = x.ServiceListing.Title,
                    ProviderCompany = x.ServiceListing.ProviderProfile.CompanyName,
                    x.RequestedStartDate,
                    x.RequestedStartTime,
                    x.EstimatedDurationHours,
                    x.EstimatedTotal,
                    x.Status
                })
                .ToListAsync(cancellationToken);

            var items = bookings.Select(x => new CustomerBookingListItemDto
            {
                Id = x.Id,
                BookingNumber = x.BookingNumber,
                ServiceTitle = x.ServiceTitle,
                ProviderCompany = x.ProviderCompany,
                RequestedStartDate = x.RequestedStartDate,
                RequestedStartTime = x.RequestedStartTime,
                EstimatedDurationHours = x.EstimatedDurationHours,
                EstimatedTotal = x.EstimatedTotal,
                Status = x.Status.ToString(),
                StatusAr = BookingDisplayHelper.GetStatusAr(x.Status),
                CanCancel = BookingActionsHelper.CanCustomerCancel(x.Status),
                CanConfirmCompletion = BookingActionsHelper.CanCustomerConfirmCompletion(x.Status),
                CanDispute = BookingActionsHelper.CanCustomerDispute(x.Status)
            }).ToList();

            return new CustomerBookingsResponseDto
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}