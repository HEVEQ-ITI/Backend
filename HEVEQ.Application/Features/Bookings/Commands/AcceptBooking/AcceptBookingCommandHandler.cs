using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using HEVEQ.Domain.Entities;
using HEVEQ.Application.Features.Bookings.Services;
using HEVEQ.Application.Features.Bookings.Helpers;

namespace HEVEQ.Application.Features.Bookings.Commands.AcceptBooking
{
    public sealed class AcceptBookingCommandHandler : IRequestHandler<AcceptBookingCommand, AcceptBookingResponseDto>
    {
        private readonly IApplicationDbContext _context;
        public AcceptBookingCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<AcceptBookingResponseDto> Handle(AcceptBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings
                .Include(b => b.ServiceListing)
                .FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);
            if (booking is null) 
                throw new InvalidOperationException("Booking not found");

            if (booking.Status != BookingStatus.PendingProviderResponse)
                throw new InvalidOperationException("Only pending bookings can be accepted.");

            var providerProfile = await _context.ProviderProfiles
                .FirstOrDefaultAsync(x => x.UserId == request.ProviderUserId, cancellationToken);
            if (providerProfile is null)
                throw new InvalidOperationException("Provider profile was not found.");

            if (booking.ServiceListing.ProviderProfileId != providerProfile.Id)
                throw new InvalidOperationException("Only the owner provider can accept this booking.");

            var operatorEntity = await _context.Operators.FirstOrDefaultAsync(
                x => x.Id == request.OperatorId &&
                x.ProviderProfileId == providerProfile.Id &&
                x.IsActive, cancellationToken);

            if (operatorEntity is null)
                throw new InvalidOperationException("Operator is not active or does not belong to this provider.");

            var operatorLinkedToListing = await _context.ServiceListingOperators.AnyAsync(x =>
                x.ListingId == booking.ServiceListingId &&
                x.OperatorId == request.OperatorId,
                cancellationToken);

            if (!operatorLinkedToListing)
                throw new InvalidOperationException("Operator is not linked to this service listing.");

            var scheduledStart = ToDateTime(booking.RequestedStartDate, booking.RequestedStartTime);
            var scheduledEnd = scheduledStart.AddHours((double)booking.EstimatedDurationHours);

            var operatorBlackout = await _context.BlackoutDates.AnyAsync(x =>
            x.ListingId == booking.ServiceListingId &&
            x.OperatorId == request.OperatorId &&
            x.Date == booking.RequestedStartDate, cancellationToken);

            if (operatorBlackout)
                throw new InvalidOperationException("Operator has a blackout date for this booking date.");

            var hasConflict = await _context.OperatorAssignments.AnyAsync(x =>
            x.OperatorId == request.OperatorId &&
            x.Status != OperatorAssignmentStatus.Cancelled &&
            x.Status != OperatorAssignmentStatus.Completed &&
            scheduledStart < x.ScheduledEnd &&
            scheduledEnd > x.ScheduledStart, cancellationToken);

            if(hasConflict)
                throw new InvalidOperationException("Operator already has another assignment in the same time range.");

            var assignment = new OperatorAssignment
            {
                BookingId = booking.Id,
                OperatorId = request.OperatorId,
                Status = OperatorAssignmentStatus.Assigned,
                ScheduledStart = scheduledStart,
                ScheduledEnd = scheduledEnd,
                CreatedAt = DateTime.Now
            };

            _context.OperatorAssignments.Add(assignment);

            booking.AssignedOperatorId = request.OperatorId;
            booking.Status = BookingStatus.ConfirmedPendingPayment;
            booking.ConfirmedAt = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);
            return new AcceptBookingResponseDto
            {
                Id = booking.Id,
                BookingNumber = booking.BookingNumber,
                Status = booking.Status.ToString(),
                StatusAr = BookingDisplayHelper.GetStatusAr(booking.Status),
                AssignedOperatorName = operatorEntity.FullName,
                Message = "Booking accepted successfully"
            };

        }

        private static DateTime ToDateTime(DateOnly date, TimeOnly time)
        {
            return date.ToDateTime(time, DateTimeKind.Utc);
        }
    }
}
