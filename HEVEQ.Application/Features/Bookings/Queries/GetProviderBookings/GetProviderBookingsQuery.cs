using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Queries.GetProviderBookings
{
    public sealed record GetProviderBookingsQuery(Guid ProviderUserId) : IRequest<IReadOnlyList<ProviderBookingListItemDto>>;
}
