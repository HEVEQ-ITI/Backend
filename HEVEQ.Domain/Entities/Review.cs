using HEVEQ.Domain.Enums;
using HEVEQ.Domain.Identity;

namespace HEVEQ.Domain.Entities;

public class Review
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ReviewerId { get; set; }

    public Guid ReviewedUserId { get; set; }

    public Guid? BookingId { get; set; }

    public Guid? ServiceListingId { get; set; }

    public Guid? MarketplaceOrderId { get; set; }

    public Guid? MarketplaceListingId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public ModerationStatus ModerationStatus { get; set; }

    public bool IsPublished { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? PublishedAt { get; set; }
    public ApplicationUser Reviewer { get; set; } = null!;

    public ApplicationUser ReviewedUser { get; set; } = null!;

    public Booking? Booking { get; set; }

    public ServiceListing? ServiceListing { get; set; }

    public MarketplaceOrder? MarketplaceOrder { get; set; }

    public MarketplaceListing? MarketplaceListing { get; set; }

}
