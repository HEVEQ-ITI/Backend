using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.Bookings.Queries.GetBookingEscrow
{
    public sealed record GetBookingEscrowQuery(Guid UserId, string? Role, Guid BookingId) : IRequest<BookingEscrowDto>;
}