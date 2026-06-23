using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Application.Features.Bookings.Services;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.DisputeBooking
{
    public sealed class DisputeBookingCommandHandler : IRequestHandler<DisputeBookingCommand, BookingDto>
    {
        private readonly IApplicationDbContext _context;

        public DisputeBookingCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BookingDto> Handle(DisputeBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings
                .Include(x => x.ServiceListing)
                .FirstOrDefaultAsync(x => x.Id == request.BookingId, cancellationToken);

            if (booking is null)
                throw new InvalidOperationException("Booking was not found.");

            if (booking.CustomerId != request.CustomerId)
                throw new InvalidOperationException("Only the booking customer can open a dispute.");

            if (booking.Status != BookingStatus.PendingCustomerConfirmation)
                throw new InvalidOperationException("Dispute can only be opened after provider marks the booking as completed.");

            booking.Status = BookingStatus.Disputed;
            booking.DisputeOpenedAt = DateTime.UtcNow;

            // TODO: When Tickets module is implemented:
            // Create a Ticket linked to this BookingId with Category = CompletionDispute,
            // store the customer's initial dispute reason as TicketMessage,
            // store any attachments as TicketAttachments,
            // and notify Admin / Customer Service queue.
            // Current MVP step only marks the booking as Disputed and blocks the normal completion flow.

            await _context.SaveChangesAsync(cancellationToken);
            return BookingDtoMapper.ToDto(booking);
        }
    }
}
