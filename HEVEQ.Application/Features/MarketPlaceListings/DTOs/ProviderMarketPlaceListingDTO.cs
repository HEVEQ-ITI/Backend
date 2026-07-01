using HEVEQ.Application.Features.MarketPlace.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceListings.DTOs
{
   public class ProviderMarketPlaceListingDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Status { get; set; } = string.Empty;
        public string StatusAr { get; set; } = string.Empty;
        public int PhotosCount { get; set; }
        public int? AiRiskScore { get; set; }
        public string? AiRiskLevel { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
