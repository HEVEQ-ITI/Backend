using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.CompleteBookingByProvider
{
    public sealed record CompleteBookingByProviderCommand(Guid ProviderUserId, Guid BookingId) : IRequest<BookingDto>;
}
