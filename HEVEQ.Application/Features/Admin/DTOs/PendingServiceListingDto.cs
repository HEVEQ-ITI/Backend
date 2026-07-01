using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.DTOs
{
    public class PendingServiceListingDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string OwnerName { get; set; }
        public string CategoryName { get; set; }
        public int PhotosCount { get; set; }
        public int OperatorsCount { get; set; }
        public int DocumentsCount { get; set; }
        public int QualityScore { get; set; }
        public int? AiRiskScore { get; set; }
        public string AiRiskLevel { get; set; }
        public string AiRiskFlags { get; set; }
        public string AiRecommendation { get; set; }
        public string Status { get; set; }
        public string StatusAr { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}
