using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Application.Features.Bookings.Services;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.RejectBooking
{
    public sealed class RejectBookingCommandHandler : IRequestHandler<RejectBookingCommand, BookingDto>
    {
        private readonly IApplicationDbContext _context;
        public RejectBookingCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BookingDto> Handle(RejectBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings
                .Include(x => x.ServiceListing)
                .FirstOrDefaultAsync(x => x.Id == request.BookingId, cancellationToken);

            if(booking is null)
                throw new InvalidOperationException("Booking was not found.");

            if (booking.Status != BookingStatus.PendingProviderResponse)
                throw new InvalidOperationException("Only pending bookings can be rejected.");

            var providerProfile = await _context.ProviderProfiles.FirstOrDefaultAsync(x => x.UserId == request.ProviderId, cancellationToken);

            if (providerProfile is null)
                throw new InvalidOperationException("Provider profile was not found.");

            if (booking.ServiceListing.ProviderProfileId != providerProfile.Id)
                throw new InvalidOperationException("Only the owner provider can reject this booking.");

            booking.Status = BookingStatus.Rejected;
            booking.ProviderRejectionReason = request.reason;

            await _context.SaveChangesAsync(cancellationToken);
            return BookingDtoMapper.ToDto(booking);
        }
    }
}
