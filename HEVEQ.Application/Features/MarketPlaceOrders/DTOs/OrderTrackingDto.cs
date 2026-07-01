using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.DTOs
{
    public class OrderTrackingDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string ListingTitle { get; set; } = string.Empty;
        public string BuyerName { get; set; } = string.Empty;
        public string SellerName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? DeliveryPreference { get; set; }
        public string? TrackingNumber { get; set; }
        public string Status { get; set; } = string.Empty;
        public string StatusAr { get; set; } = string.Empty;
        public string? EscrowStatus { get; set; }
        public List<OrderTrackingTimelineItemDto> Timeline { get; set; } = new();
        public OrderTrackingActionsDto AvailableActions { get; set; } = new();
    }
}
