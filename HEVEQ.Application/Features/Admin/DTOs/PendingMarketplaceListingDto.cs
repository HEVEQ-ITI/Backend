using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.DTOs
{
    public class PendingMarketplaceListingDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string OwnerName { get; set; }
        public string CategoryName { get; set; }
        public int? AiRiskScore { get; set; }
        public string AiRiskLevel { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}
