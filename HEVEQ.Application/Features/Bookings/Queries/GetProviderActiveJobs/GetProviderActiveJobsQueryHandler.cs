using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Application.Features.Bookings.Helpers;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HEVEQ.Application.Features.Bookings.Queries.GetProviderActiveJobs
{
    public sealed class GetProviderActiveJobsQueryHandler : IRequestHandler<GetProviderActiveJobsQuery, ProviderActiveJobsResponseDto>
    {
        private readonly IApplicationDbContext _context;
        public GetProviderActiveJobsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProviderActiveJobsResponseDto> Handle(GetProviderActiveJobsQuery request, CancellationToken cancellationToken)
        {
            var providerProfile = await _context.ProviderProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == request.ProviderUserId, cancellationToken);

            if (providerProfile is null)
                throw new InvalidOperationException("Provider profile was not found.");

            var bookings = await _context.Bookings
                .AsNoTracking()
                .Where(x =>
                    x.ServiceListing.ProviderProfileId == providerProfile.Id &&
                    (
                        x.Status == BookingStatus.Active ||
                        x.Status == BookingStatus.InProgress ||
                        x.Status == BookingStatus.PendingCustomerConfirmation
                    ))
                .OrderBy(x => x.RequestedStartDate)
                .ThenBy(x => x.RequestedStartTime)
                .Select(x => new
                {
                    x.Id,
                    x.BookingNumber,
                    ServiceTitle = x.ServiceListing.Title,
                    CustomerName = x.Customer.FirstName + " " + x.Customer.LastName,
                    OperatorName = x.AssignedOperator == null ? string.Empty : x.AssignedOperator.FullName,
                    x.RequestedStartDate,
                    x.RequestedStartTime,
                    x.EstimatedDurationHours,
                    x.Status,
                    x.AssignedOperatorId
                })
                .ToListAsync(cancellationToken);

            var items = bookings.Select(x =>
            {
                var scheduledStart = x.RequestedStartDate.ToDateTime(x.RequestedStartTime);
                var scheduledEnd = scheduledStart.AddHours((double)x.EstimatedDurationHours);

                return new ProviderActiveJobItemDto
                {
                    Id = x.Id,
                    BookingNumber = x.BookingNumber,
                    ServiceTitle = x.ServiceTitle,
                    CustomerName = x.CustomerName.Trim(),
                    OperatorName = x.OperatorName,
                    ScheduledStart = scheduledStart,
                    ScheduledEnd = scheduledEnd,
                    Status = x.Status.ToString(),
                    StatusAr = BookingDisplayHelper.GetStatusAr(x.Status),
                    CanStart = BookingActionsHelper.CanProviderStart(x.Status, x.AssignedOperatorId),
                    CanComplete = BookingActionsHelper.CanProviderComplete(x.Status)
                };
            }).ToList();

            return new ProviderActiveJobsResponseDto
            {
                Items = items
            };
        }
    }
}