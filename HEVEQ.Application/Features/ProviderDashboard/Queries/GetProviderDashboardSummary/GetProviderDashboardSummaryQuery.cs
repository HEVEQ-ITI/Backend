using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ProviderDashboard.Queries.GetProviderDashboardSummary
{
    public record GetProviderDashboardSummaryQuery : IRequest<ProviderDashboardSummaryDto>;
}
