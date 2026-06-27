using HEVEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.DTOs
{
    public class MarketplaceOrderDto
    {
        public Guid Id { get; set; }
        public Guid BuyerId { get; set; }
        public string BuyerName { get; set; } = string.Empty;
        public Guid SellerId { get; set; }
        public string SellerName { get; set; } = string.Empty;
        public Guid ListingId { get; set; }
        public string ListingTitle { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? DeliveryPreference { get; set; }
        public string? TrackingNumber { get; set; }
        public string Status { get; set; } = string.Empty;
        public string StatusAr { get; set; } = string.Empty;
        public DateTime? SellerConfirmedAt { get; set; }
        public DateTime? DispatchedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? ConfirmedByBuyerAt { get; set; }
        public string? CancellationReason { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string? CancellationInitiatedByRole { get; set; }
        public decimal? ReturnShippingCost { get; set; }
        public DateTime? ReturnShippingAcceptedByBuyerAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ViewerRole { get; set; } = string.Empty;

    }
}
