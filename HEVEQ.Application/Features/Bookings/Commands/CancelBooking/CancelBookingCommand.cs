using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.CancelBooking
{
    public sealed record CancelBookingCommand(Guid UserId, Guid BookingId, string reason) : IRequest<BookingDto>;
}
