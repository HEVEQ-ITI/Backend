using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.DTOs
{
    public class PaginatedAdminDisputesResponse
    {
        public List<AdminDisputeDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
