using HEVEQ.Application.Features.Admin.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Query.GetAdminTickets
{
    public class GetAdminTicketsQuery : IRequest<PaginatedAdminTicketsResponse>
    {
        public string Status { get; set; }
        public string Category { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
