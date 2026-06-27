using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Queries.GetProviderActiveJobs
{
    public sealed record GetProviderActiveJobsQuery(Guid ProviderUserId) : IRequest<ProviderActiveJobsResponseDto>;
}
