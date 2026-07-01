using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Application.Features.Bookings.Helpers;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HEVEQ.Application.Features.Bookings.Commands.DisputeBooking
{
    public sealed class DisputeBookingCommandHandler : IRequestHandler<DisputeBookingCommand, DisputeBookingResponseDto>
    {
        private readonly IApplicationDbContext _context;
        public DisputeBookingCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DisputeBookingResponseDto> Handle(DisputeBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(x => x.Id == request.BookingId, cancellationToken);

            if (booking is null)
                throw new InvalidOperationException("Booking was not found.");

            if (booking.CustomerId != request.CustomerId)
                throw new InvalidOperationException("Only the booking customer can open a dispute.");

            if (booking.Status != BookingStatus.PendingCustomerConfirmation)
                throw new InvalidOperationException("Booking must be pending customer confirmation before opening a dispute.");

            booking.Status = BookingStatus.Disputed;
            booking.DisputeOpenedAt = DateTime.Now;

            var escrow = await _context.EscrowRecords.FirstOrDefaultAsync(x => x.BookingId == booking.Id, cancellationToken);

            if (escrow is not null && escrow.Status == EscrowStatus.Held) { 
                escrow.Status = EscrowStatus.Frozen;
                escrow.FrozenAt = DateTime.Now;
                escrow.FreezeReason = request.Reason;
            } 
            //TODO: Notification
            // TODO: Create Ticket with Category = CompletionDispute when Tickets feature is ready.
            // Ticket should store BookingId, OpenedByUserId, Reason, EvidencePhotoUrls, Status = Open.


            await _context.SaveChangesAsync(cancellationToken);
            return new DisputeBookingResponseDto
            {
                BookingId = booking.Id,
                Status = booking.Status.ToString(),
                StatusAr = BookingDisplayHelper.GetStatusAr(booking.Status),
                TicketId = null,
                Message = "Dispute opened successfully"
            };
        }
    }
}