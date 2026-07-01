using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceListings.DTOs
{
    public class ListingSellerDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public decimal? AverageRating { get; set; }
        public int? TotalReviewsCount { get; set; }
    }
}
