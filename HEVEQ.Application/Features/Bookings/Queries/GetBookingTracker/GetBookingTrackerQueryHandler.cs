using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Application.Features.Bookings.Helpers;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HEVEQ.Application.Features.Bookings.Queries.GetBookingTracker
{
    public sealed class GetBookingTrackerQueryHandler : IRequestHandler<GetBookingTrackerQuery, BookingTrackerDto>
    {
        private readonly IApplicationDbContext _context;
        public GetBookingTrackerQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BookingTrackerDto> Handle(GetBookingTrackerQuery request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings
                .AsNoTracking()
                .Include(x => x.ServiceListing)
                    .ThenInclude(x => x.ProviderProfile)
                .FirstOrDefaultAsync(x => x.Id == request.BookingId, cancellationToken);

            if (booking is null)
                throw new InvalidOperationException("Booking was not found.");

            var isCustomer = string.Equals(request.Role, "Customer", StringComparison.OrdinalIgnoreCase);
            var isProvider = string.Equals(request.Role, "Provider", StringComparison.OrdinalIgnoreCase);
            var isAdmin = string.Equals(request.Role, "Admin", StringComparison.OrdinalIgnoreCase);

            var isCustomerOwner = isCustomer && booking.CustomerId == request.UserId;

            var isProviderOwner =
                isProvider &&
                booking.ServiceListing != null &&
                booking.ServiceListing.ProviderProfile != null &&
                booking.ServiceListing.ProviderProfile.UserId == request.UserId;

            if (!isCustomerOwner && !isProviderOwner && !isAdmin)
                throw new UnauthorizedAccessException("You are not allowed to view tracker for this booking.");

            var availableActions = new BookingActionsDto
            {
                CanAccept = isProvider && BookingActionsHelper.CanProviderAccept(booking.Status),
                CanReject = isProvider && BookingActionsHelper.CanProviderReject(booking.Status),
                CanStart = isProvider && BookingActionsHelper.CanProviderStart(booking.Status, booking.AssignedOperatorId),
                CanComplete = isProvider && BookingActionsHelper.CanProviderComplete(booking.Status),
                CanConfirmCompletion = isCustomer && BookingActionsHelper.CanCustomerConfirmCompletion(booking.Status),
                CanDispute = isCustomer && BookingActionsHelper.CanCustomerDispute(booking.Status),
                CanCancel = isCustomer && BookingActionsHelper.CanCustomerCancel(booking.Status),
                CanProviderCancel = isProvider && BookingActionsHelper.CanProviderCancel(booking.Status),
                CanPay = isCustomer && BookingActionsHelper.CanCustomerPay(booking.Status)
            };

            var nextAction = BookingNextActionHelper.Build(booking.Status, request.Role);

            return new BookingTrackerDto
            {
                BookingId = booking.Id,
                BookingNumber = booking.BookingNumber,
                CurrentStatus = booking.Status.ToString(),
                CurrentStatusAr = BookingDisplayHelper.GetStatusAr(booking.Status),
                Timeline = BookingTimelineHelper.Build(booking),
                NextAction = new BookingNextActionDto
                {
                    Label = nextAction.Label,
                    LabelAr = BookingNextActionHelper.Build(booking.Status, request.Role).LabelAr,
                    ActionKey = nextAction.ActionKey
                },
                AvailableActions = availableActions
            };
        }
    }
}