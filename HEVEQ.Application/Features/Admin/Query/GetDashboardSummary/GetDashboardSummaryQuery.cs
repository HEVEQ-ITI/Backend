using HEVEQ.Application.Features.Admin.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Query.GetDashboardSummary
{
    public class GetDashboardSummaryQuery : IRequest<AdminDashboardSummaryDTO>
    {
    }
}
