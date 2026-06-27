using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Queries.GetProviderBookingRequests
{
    public sealed record GetProviderBookingRequestsQuery(Guid ProviderUserId) : IRequest<ProviderBookingRequestsResponseDto>;
}
