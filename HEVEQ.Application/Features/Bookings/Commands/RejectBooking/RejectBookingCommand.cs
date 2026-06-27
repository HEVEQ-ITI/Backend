using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.RejectBooking
{
    public sealed record RejectBookingCommand(Guid ProviderId, Guid BookingId, string reason) : IRequest<RejectBookingResponseDto>;
}
