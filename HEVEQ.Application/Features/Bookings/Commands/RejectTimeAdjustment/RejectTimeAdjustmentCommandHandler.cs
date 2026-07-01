using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Application.Features.Bookings.Helpers;
using HEVEQ.Application.Features.Bookings.Services;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.RejectTimeAdjustment
{
    public sealed class RejectTimeAdjustmentCommandHandler : IRequestHandler<RejectTimeAdjustmentCommand, TimeAdjustmentDecisionResponseDto>
    {
        private readonly IApplicationDbContext _context;
        public RejectTimeAdjustmentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TimeAdjustmentDecisionResponseDto> Handle(RejectTimeAdjustmentCommand request, CancellationToken cancellationToken)
        {
            var adjustmentRequest = await _context.BookingTimeAdjustmentRequests
                .Include(x => x.Booking)
                    .ThenInclude(x => x.ServiceListing)
                .FirstOrDefaultAsync(x =>
                    x.Id == request.TimeAdjustmentRequestId,
                    cancellationToken);

            if (adjustmentRequest is null)
                throw new InvalidOperationException("Time adjustment request was not found.");

            var booking = adjustmentRequest.Booking;

            if (booking is null)
                throw new InvalidOperationException("Related booking was not found.");

            if (booking.CustomerId != request.CustomerId)
                throw new InvalidOperationException("Only the booking customer can reject this time adjustment request.");

            if (adjustmentRequest.Status != BookingTimeAdjustmentStatus.Pending)
                throw new InvalidOperationException("Only pending time adjustment requests can be rejected.");

            if (booking.Status != BookingStatus.Active && booking.Status != BookingStatus.InProgress && booking.Status != BookingStatus.PendingCustomerConfirmation)
                throw new InvalidOperationException("Time adjustment can only be rejected before booking completion.");

            adjustmentRequest.Status = BookingTimeAdjustmentStatus.Rejected;
            adjustmentRequest.CustomerAcknowledgedAt = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);
            return new TimeAdjustmentDecisionResponseDto
            {
                Id = adjustmentRequest.Id,
                BookingId = booking.Id,
                BookingNumber = booking.BookingNumber,
                RequestedAdditionalHrs = adjustmentRequest.RequestedAdditionalHrs,
                AdditionalCostAmount = adjustmentRequest.AdditionalCostAmount,
                BookingEstimatedDurationHours = booking.EstimatedDurationHours,
                BookingEstimatedTotal = booking.EstimatedTotal,
                Status = adjustmentRequest.Status.ToString(),
                StatusAr = TimeAdjustmentDisplayHelper.GetStatusAr(adjustmentRequest.Status),
                CustomerAcknowledgedAt = adjustmentRequest.CustomerAcknowledgedAt,
                Message = "Time adjustment request rejected successfully"
            };
        }
    }
}
