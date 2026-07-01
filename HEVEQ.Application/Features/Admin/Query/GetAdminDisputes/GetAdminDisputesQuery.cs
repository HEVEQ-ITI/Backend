using HEVEQ.Application.Features.Admin.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Query.GetAdminDisputes
{
    public class GetAdminDisputesQuery : IRequest<PaginatedAdminDisputesResponse>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
