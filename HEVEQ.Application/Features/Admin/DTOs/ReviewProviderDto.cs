using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.DTOs
{
    public class ReviewProviderDto
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
    
    public class ReviewPhotoDto { public Guid Id { get; set; } public string Url { get; set; } }
    public class ReviewOperatorDto { public Guid Id { get; set; } public string Name { get; set; } }
    public class ReviewAvailabilityDto { public string Day { get; set; } public string Hours { get; set; } }
    public class ReviewDocumentDto { public Guid Id { get; set; } public string Type { get; set; } public string FileUrl { get; set; } }

    public class ServiceListingReviewDetailsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public ReviewProviderDto Provider { get; set; }
        public ReviewPricingDto Pricing { get; set; }

        public List<ReviewPhotoDto> Photos { get; set; } = new();
        public List<ReviewOperatorDto> Operators { get; set; } = new();
        public List<ReviewAvailabilityDto> Availability { get; set; } = new();
        public List<ReviewDocumentDto> Documents { get; set; } = new();

        public int QualityScore { get; set; }
        public int? AiRiskScore { get; set; }
        public string AiRiskLevel { get; set; }
        public string AiRiskFlags { get; set; }
        public string AiRecommendation { get; set; }

        public string Status { get; set; }
        public string StatusAr { get; set; }
    }
}
