using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Application.Features.Bookings.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Queries.GetBookingById
{
    public sealed class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingDto>
    {
        private readonly IApplicationDbContext _context;
        public GetBookingByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BookingDto> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings
                .Include(x => x.ServiceListing)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.BookingId, cancellationToken);

            if (booking is null)
                throw new InvalidOperationException("Booking was not found.");

            var isCustomerOwner = booking.CustomerId == request.UserId;

            var providerProfile = await _context.ProviderProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

            var isProviderOwner = providerProfile is not null && booking.ServiceListing.ProviderProfileId == providerProfile.Id;

            if (!isCustomerOwner && !isProviderOwner)
                throw new InvalidOperationException("You are not allowed to view this booking.");

            return BookingDtoMapper.ToDto(booking);
        }
    }
}
