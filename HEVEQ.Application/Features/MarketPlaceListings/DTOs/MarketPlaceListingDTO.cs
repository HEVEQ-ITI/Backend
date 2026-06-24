using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using HEVEQ.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.DTOs
{
    public class MarketPlaceListingDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public MarketplaceTransactionMethod TransactionMethod { get; set; } 
        public ProductCondition Condition { get; set; }
        public MarketplaceListingStatus Status { get; set; } 

        public string? Governorate { get; set; }
        public string? District { get; set; }

        public string Category { get; set; } = string.Empty;
        public int CategoryId { get; set; }

        public string Description { get; set; } = string.Empty;
        public bool IsNegotiable { get; set; }
        public int? YearOfManufacture { get; set; }
        public string? Specifications { get; set; }

        public Guid SellerId { get; set; }
        public string SellerName { get; set; } = string.Empty;

        public string? CoverPhotoUrl { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}

