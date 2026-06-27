using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Application.Features.Bookings.Services;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Features.Bookings.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.StartBooking
{
    public sealed class StartBookingCommandHandler : IRequestHandler<StartBookingCommand, StartBookingResponseDto>
    {
        private readonly IApplicationDbContext _context;
        public StartBookingCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<StartBookingResponseDto> Handle(StartBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings
                .Include(b => b.ServiceListing)
                .FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

            if (booking is null)
                throw new InvalidOperationException("Booking was not found.");

            if (booking.Status != BookingStatus.Active)
                throw new InvalidOperationException("Booking can only be started when it is active.");

            var providerProfile = await _context.ProviderProfiles.FirstOrDefaultAsync(p => p.UserId == request.ProviderUserId, cancellationToken);

            if (providerProfile is null)
                throw new InvalidOperationException("Provider profile was not found.");

            if (booking.ServiceListing.ProviderProfileId != providerProfile.Id)
                throw new InvalidOperationException("Only the owner provider can start this booking.");

            if (booking.AssignedOperatorId is null)
                throw new InvalidOperationException("Booking cannot be started without an assigned operator.");

            var assignment = await _context.OperatorAssignments.FirstOrDefaultAsync(a => a.BookingId == booking.Id && a.OperatorId == booking.AssignedOperatorId, cancellationToken);

            if (assignment is null)
                throw new InvalidOperationException("Operator assignment was not found for this booking.");

            if (assignment.Status == OperatorAssignmentStatus.Cancelled)
                throw new InvalidOperationException("Cancelled operator assignment cannot be started.");

            if (assignment.Status == OperatorAssignmentStatus.Completed)
                throw new InvalidOperationException("Completed operator assignment cannot be started.");

            booking.Status = BookingStatus.InProgress;
            booking.StartedAt = DateTime.Now;
            assignment.Status = OperatorAssignmentStatus.InProgress;

            await _context.SaveChangesAsync(cancellationToken);
            return new StartBookingResponseDto
            {
                Id = booking.Id,
                BookingNumber = booking.BookingNumber,
                Status = booking.Status.ToString(),
                StatusAr = BookingDisplayHelper.GetStatusAr(booking.Status),
                StartedAt = booking.StartedAt,
                Message = "Booking started successfully"
            };
        }
    }
}
