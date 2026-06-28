using FluentValidation;
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

namespace HEVEQ.Application.Features.Bookings.Commands.ConfirmBookingCompletion
{
    public sealed class ConfirmBookingCompletionCommandHandler : IRequestHandler<ConfirmBookingCompletionCommand, ConfirmBookingCompletionResponseDto>
    {
        private readonly IApplicationDbContext _context;
        public ConfirmBookingCompletionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ConfirmBookingCompletionResponseDto> Handle(ConfirmBookingCompletionCommand request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings
                .Include(x => x.ServiceListing)
                .FirstOrDefaultAsync(x => x.Id == request.BookingId, cancellationToken);

            if (booking is null)
                throw new InvalidOperationException("Booking was not found.");

            if (booking.CustomerId != request.CustomerId)
                throw new InvalidOperationException("Only the booking customer can confirm completion.");

            if (booking.Status != BookingStatus.PendingCustomerConfirmation)
                throw new InvalidOperationException("Booking can only be confirmed after provider marks it as completed.");

            // Any pending time adjustment request expires when the customer confirms completion,
            // because the booking is now closed and no additional cost can be approved after completion.
            var pendingAdjustments = await _context.BookingTimeAdjustmentRequests
                .Where(x =>
                    x.BookingId == booking.Id &&
                    x.Status == BookingTimeAdjustmentStatus.Pending)
                .ToListAsync(cancellationToken);

            foreach (var adjustment in pendingAdjustments)
            {
                adjustment.Status = BookingTimeAdjustmentStatus.Expired;
            }
            booking.Status = BookingStatus.Completed;
            booking.CompletionConfirmedAt = DateTime.Now;

            //Payment will be done later

            await _context.SaveChangesAsync(cancellationToken);
            return new ConfirmBookingCompletionResponseDto
            {
                Id = booking.Id,
                BookingNumber = booking.BookingNumber,
                Status = booking.Status.ToString(),
                StatusAr = BookingDisplayHelper.GetStatusAr(booking.Status),
                CompletionConfirmedAt = booking.CompletionConfirmedAt,
                Message = "Booking completion confirmed successfully"
            };
        }
    }
}
