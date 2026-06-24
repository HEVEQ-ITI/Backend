using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.AcceptBooking
{
    public sealed record AcceptBookingCommand(Guid ProviderUserId, Guid BookingId, Guid OperatorId) : IRequest<BookingDto>;
}
