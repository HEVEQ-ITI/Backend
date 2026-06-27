using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;
using HEVEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Queries.GetCustomerBookings
{
    public sealed record GetCustomerBookingsQuery(Guid CustomerId, BookingStatus? Status) : IRequest<CustomerBookingsResponseDto>;
}
