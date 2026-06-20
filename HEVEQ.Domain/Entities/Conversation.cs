using HEVEQ.Domain.Identity;
namespace HEVEQ.Domain.Entities;

public class Conversation
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid? ServiceListingId { get; set; }

    public Guid? MarketplaceListingId { get; set; }

    public Guid? BookingId { get; set; }

    public Guid InitiatedById { get; set; }

    public Guid ParticipantId { get; set; }

    public bool IsLocked { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LockedAt { get; set; }
    public ServiceListing? ServiceListing { get; set; }

    public MarketplaceListing? MarketplaceListing { get; set; }

    public Booking? Booking { get; set; }

    public ApplicationUser InitiatedBy { get; set; } = null!;

    public ApplicationUser Participant { get; set; } = null!;

    public ICollection<Message> Messages { get; set; } = new List<Message>();

    public ICollection<ConversationReadReceipt> ReadReceipts { get; set; } = new List<ConversationReadReceipt>();

}
