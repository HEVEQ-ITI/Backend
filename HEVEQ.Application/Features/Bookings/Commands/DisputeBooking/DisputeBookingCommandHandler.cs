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

            if (string.IsNullOrWhiteSpace(request.Reason))
                throw new InvalidOperationException("Dispute reason is required.");

            var evidencePhotoUrls = request.EvidencePhotoUrls ?? Array.Empty<string>();

            booking.Status = BookingStatus.Disputed;
            booking.DisputeOpenedAt = DateTime.UtcNow;

            // TODO: Create Ticket with Category = CompletionDispute when Tickets feature is ready.
            // Ticket should store BookingId, OpenedByUserId, Reason, EvidencePhotoUrls, Status = Open.

            // TODO: Freeze escrow release when payment and escrow module is implemented.

            await _context.SaveChangesAsync(cancellationToken);
            return new DisputeBookingResponseDto
            {
                Id = booking.Id,
                BookingNumber = booking.BookingNumber,
                Status = booking.Status.ToString(),
                StatusAr = BookingDisplayHelper.GetStatusAr(booking.Status),
                DisputeOpenedAt = booking.DisputeOpenedAt,
                TicketId = null,
                Message = "Dispute opened successfully"
            };
        }
    }
}