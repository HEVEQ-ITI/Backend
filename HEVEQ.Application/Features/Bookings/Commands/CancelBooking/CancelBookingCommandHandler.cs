using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Application.Features.Bookings.Services;
using HEVEQ.Application.Features.Bookings.Services.Interfaces;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.CancelBooking
{
    public sealed class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, BookingDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICancellationPolicyService _policyService;
        public CancelBookingCommandHandler(IApplicationDbContext context, ICancellationPolicyService policyService)
        {
            _context = context;
            _policyService = policyService;
        }
        public async Task<BookingDto> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings
                .Include(b => b.ServiceListing)
                .FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

            if (booking is null)
                throw new InvalidOperationException("Booking not found.");

            _policyService.EnsureCanBeCancelled(booking.Status);

            var initiator = await _policyService.ResolveActorAsync(booking, request.UserId, cancellationToken);
            var refundPct = _policyService.CalculateRefundPercentage(booking.Status);

            booking.Status = booking.PaymentCapturedAt is not null ? BookingStatus.CancelledRefunded : BookingStatus.Cancelled;
            booking.CancellationReason = request.reason;
            booking.CancelledAt = DateTime.UtcNow;
            booking.CancellationInitiatedByRole = initiator;
            booking.CancellationRefundPct = refundPct;

            if (initiator == BookingCancellationInitiator.Provider && booking.PaymentCapturedAt is not null)
                booking.ProviderCancellationPenaltyApplied = true;

            var assignment = await _context.OperatorAssignments
                .FirstOrDefaultAsync(x => x.BookingId == booking.Id, cancellationToken);

            if (assignment is not null && assignment.Status != OperatorAssignmentStatus.Completed && assignment.Status != OperatorAssignmentStatus.Cancelled)
            {
                assignment.Status = OperatorAssignmentStatus.Cancelled;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return BookingDtoMapper.ToDto(booking);
        }
    }
}
