using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.ConfirmBookingCompletion
{
    public sealed record ConfirmBookingCompletionCommand(Guid CustomerId, Guid BookingId) : IRequest<BookingDto>;
}
