using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceListings.DTOs
{
    public class ListingManagementInfoDto
    {
        public int? AiRiskScore { get; set; }
        public string? AiRiskLevel { get; set; }
        public string? AiRiskFlags { get; set; }
        public string? AdminRejectionNote { get; set; }
    }
}
