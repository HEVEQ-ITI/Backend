using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.DTOs
{
    public class AdminTicketDto
    {
        public Guid Id { get; set; }
        public string TicketNumber { get; set; }
        public string Subject { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string StatusAr { get; set; }
        public string SubmittedByName { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class PaginatedAdminTicketsResponse
    {
        public List<AdminTicketDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
