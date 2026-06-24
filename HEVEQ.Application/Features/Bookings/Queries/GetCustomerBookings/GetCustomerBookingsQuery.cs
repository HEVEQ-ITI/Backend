using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Queries.GetCustomerBookings
{
    public sealed record GetCustomerBookingsQuery(Guid CustomerId) : IRequest<IReadOnlyList<CustomerBookingListItemDto>>;
}
