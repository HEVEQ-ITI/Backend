using HEVEQ.Domain.Enums;
using HEVEQ.Domain.Identity;

namespace HEVEQ.Domain.Entities;

public class MarketplaceOrder
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string OrderNumber { get; set; } = string.Empty;

    public Guid BuyerId { get; set; }

    public Guid ListingId { get; set; }

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
    public ApplicationUser Buyer { get; set; } = null!;

    public MarketplaceListing Listing { get; set; } = null!;

    public ICollection<EscrowRecord> EscrowRecords { get; set; } = new List<EscrowRecord>();

    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

}
