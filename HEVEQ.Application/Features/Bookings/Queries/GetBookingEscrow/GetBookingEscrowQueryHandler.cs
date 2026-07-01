using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Application.Features.Bookings.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HEVEQ.Application.Features.Bookings.Queries.GetBookingEscrow
{
    public sealed class GetBookingEscrowQueryHandler : IRequestHandler<GetBookingEscrowQuery, BookingEscrowDto>
    {
        private readonly IApplicationDbContext _context;
        public GetBookingEscrowQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BookingEscrowDto> Handle(GetBookingEscrowQuery request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings
                .AsNoTracking()
                .Include(x => x.ServiceListing)
                    .ThenInclude(x => x.ProviderProfile)
                .Include(x => x.EscrowRecords)
                .FirstOrDefaultAsync(x => x.Id == request.BookingId, cancellationToken);

            if (booking is null)
                throw new InvalidOperationException("Booking was not found.");

            var isCustomer = string.Equals(request.Role, "Customer", StringComparison.OrdinalIgnoreCase);
            var isProvider = string.Equals(request.Role, "Provider", StringComparison.OrdinalIgnoreCase);
            var isAdmin = string.Equals(request.Role, "Admin", StringComparison.OrdinalIgnoreCase);

            var isCustomerOwner = isCustomer && booking.CustomerId == request.UserId;

            var isProviderOwner =
                isProvider &&
                booking.ServiceListing != null &&
                booking.ServiceListing.ProviderProfile != null &&
                booking.ServiceListing.ProviderProfile.UserId == request.UserId;

            if (!isCustomerOwner && !isProviderOwner && !isAdmin)
                throw new UnauthorizedAccessException("You are not allowed to view escrow for this booking.");

            var escrow = booking.EscrowRecords
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault();

            if (escrow is null)
            {
                return new BookingEscrowDto
                {
                    BookingId = booking.Id,
                    GrossAmount = 0,
                    PlatformCommission = 0,
                    ProviderPayout = 0,
                    VatAmount = 0,
                    Status = "NotCaptured",
                    StatusAr = "لم يتم التحصيل",
                    CapturedAt = null,
                    ReleasedAt = null,
                    FrozenAt = null
                };
            }

            return new BookingEscrowDto
            {
                BookingId = booking.Id,
                GrossAmount = escrow.GrossAmount,
                PlatformCommission = escrow.PlatformCommission,
                ProviderPayout = escrow.ProviderPayout,
                VatAmount = escrow.VatAmount,
                Status = escrow.Status.ToString(),
                StatusAr = EscrowDisplayHelper.GetStatusAr(escrow.Status),
                CapturedAt = escrow.CapturedAt,
                ReleasedAt = escrow.ReleasedAt,
                FrozenAt = escrow.FrozenAt
            };
        }
    }
}