using HEVEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.DTOs
{
    public class PurchaseOrderDto
    {
        public Guid Id { get; set; }
        public Guid ListingId { get; set; }
        public string ListingTitle { get; set; } = string.Empty;
        public Guid SellerId { get; set; }
        public string SellerName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? DeliveryAddress { get; set; }
        public DeliveryPreference? DeliveryPreference { get; set; }
        public string? TrackingNumber { get; set; }
        public MarketplaceOrderStatus Status { get; set; }
        public DateTime? SellerConfirmedAt { get; set; }
        public DateTime? DispatchedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? ConfirmedByBuyerAt { get; set; }
        public string? CancellationReason { get; set; }
        public DateTime? CancelledAt { get; set; }
        public MarketplaceOrderCancellationInitiator? CancellationInitiatedByRole { get; set; }
        public decimal? ReturnShippingCost { get; set; }
        public DateTime? ReturnShippingAcceptedByBuyerAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
