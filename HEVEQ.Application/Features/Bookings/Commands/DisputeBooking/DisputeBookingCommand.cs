using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.DisputeBooking
{
    public sealed record DisputeBookingCommand(Guid CustomerId, Guid BookingId) : IRequest<BookingDto>;
}
