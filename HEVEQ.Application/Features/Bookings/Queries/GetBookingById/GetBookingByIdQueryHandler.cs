using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Application.Features.Bookings.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.AssignedOperator)
                .Include(x => x.EscrowRecords)
                .Include(x => x.ServiceListing)
                    .ThenInclude(x => x.ProviderProfile)
                .FirstOrDefaultAsync(x => x.Id == request.BookingId, cancellationToken);

            if (booking is null)
                throw new InvalidOperationException("Booking was not found.");

            var isAdmin = string.Equals(request.Role, "Admin", StringComparison.OrdinalIgnoreCase);

            var isCustomerOwner = booking.CustomerId == request.UserId;

            var isProviderOwner = await _context.ProviderProfiles
                .AsNoTracking()
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.Id == booking.ServiceListing.ProviderProfileId,
                    cancellationToken);

            if (!isAdmin && !isCustomerOwner && !isProviderOwner)
                throw new InvalidOperationException("You are not allowed to view this booking.");

            return BookingDtoMapper.ToDto(booking, request.Role);
        }
    }
}