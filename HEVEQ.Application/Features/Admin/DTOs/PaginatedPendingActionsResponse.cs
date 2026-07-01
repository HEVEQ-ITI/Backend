using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.DTOs
{
    public class PaginatedPendingActionsResponse
    {
        public List<PendingActionDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
