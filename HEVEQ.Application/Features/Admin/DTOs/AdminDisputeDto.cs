using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.DTOs
{
    public class AdminDisputeDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string ReferenceNumber { get; set; }
        public string CustomerName { get; set; }
        public string ProviderName { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string StatusAr { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
