using HEVEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.DTOs
{
    public class MarketplaceListingDetailsDto
    {
        public Guid Id { get; set; }
        public Guid SellerId { get; set; }
        public string SellerName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;
        public string ConditionAr { get; set; } = string.Empty;
        public int? YearOfManufacture { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? Specifications { get; set; }
        public decimal Price { get; set; }
        public bool IsNegotiable { get; set; }
        public string TransactionMethod { get; set; } = string.Empty;
        public string? Governorate { get; set; }
        public string? District { get; set; }
        public string Status { get; set; } = string.Empty;
        public string StatusAr { get; set; } = string.Empty;
        public string? VideoUrl { get; set; }
        public string? AdminRejectionNote { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<MarketplaceListingPhotoDto> Photos { get; set; } = new();

    }
}
