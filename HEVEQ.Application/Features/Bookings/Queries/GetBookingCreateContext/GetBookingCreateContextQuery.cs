using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Queries.GetBookingCreateContext
{
    public sealed record GetBookingCreateContextQuery(Guid CustomerId, Guid ServiceListingId) : IRequest<BookingCreateContextDto>;
}
