using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.StartBooking
{
    public sealed record StartBookingCommand(Guid ProviderUserId, Guid BookingId) : IRequest<StartBookingResponseDto>;
}
