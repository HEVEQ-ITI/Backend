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
        public string Condition { get; set; } = string.Empty;
        public string ConditionAr { get; set; } = string.Empty;
        public string? Location { get; set; }
        public string? CoverPhotoUrl { get; set; }
        public string SellerName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string TransactionMethod { get; set; } = string.Empty;
        public decimal? AverageRating { get; set; }
        public int? TotalReviewsCount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string StatusAr { get; set; } = string.Empty;


    }
}

