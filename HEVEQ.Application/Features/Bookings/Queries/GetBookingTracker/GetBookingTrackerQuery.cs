using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.Bookings.Queries.GetBookingTracker
{
    public sealed record GetBookingTrackerQuery(Guid UserId, string? Role, Guid BookingId) : IRequest<BookingTrackerDto>;
}