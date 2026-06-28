using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Queries.GetBookingById
{
    public sealed record GetBookingByIdQuery(Guid UserId, string? Role, Guid BookingId) : IRequest<BookingDto>;
}
