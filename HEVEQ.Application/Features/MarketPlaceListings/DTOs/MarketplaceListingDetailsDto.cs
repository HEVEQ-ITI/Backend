using HEVEQ.Application.Features.MarketPlaceListings.DTOs;
using HEVEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HEVEQ.Application.Features.MarketPlace.DTOs
{
    public class MarketplaceListingDetailsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Condition { get; set; } = string.Empty;
        public string ConditionAr { get; set; } = string.Empty;
        public string? Specifications { get; set; }
        public List<MarketplaceListingPhotoDto> Photos { get; set; } = new();
        public ListingSellerDto Seller { get; set; } = new();
        public bool CanBuyNow { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ListingManagementInfoDto? ManagementInfo { get; set; }

    }
}
