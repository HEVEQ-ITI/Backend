using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.DTOs
{
    public class MarketplaceListingReviewDetailsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }

        public ReviewSellerDto Seller { get; set; }

        public decimal Price { get; set; }
        public string Condition { get; set; }
        public string ConditionAr { get; set; }

        public List<ReviewPhotoDto> Photos { get; set; } = new();
        public List<ReviewDocumentDto> Documents { get; set; } = new();

        public int? AiRiskScore { get; set; }
        public string AiRiskLevel { get; set; }
        public string AiRiskFlags { get; set; }

        public string Status { get; set; }
        public string StatusAr { get; set; }
    }
}
