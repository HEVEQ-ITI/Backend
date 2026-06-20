using HEVEQ.Domain.Enums;
using HEVEQ.Domain.Identity;

namespace HEVEQ.Domain.Entities;

public class Ticket
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string TicketNumber { get; set; } = string.Empty;

    public Guid SubmittedById { get; set; }

    public Guid? AssignedToUserId { get; set; }

    public Guid? ResolvedByUserId { get; set; }

    public Guid? BookingId { get; set; }

    public Guid? MarketplaceOrderId { get; set; }

    public string Subject { get; set; } = string.Empty;

    public TicketCategory Category { get; set; }

    public int Priority { get; set; }

    public TicketStatus Status { get; set; }

    public string? AiSummary { get; set; }

    public string? AiIdentifiedIssue { get; set; }

    public string? AiClaimedImpact { get; set; }

    public int? AiEscalationPriority { get; set; }

    public TicketResolutionType? ResolutionType { get; set; }

    public string? AdminResolution { get; set; }

    public DateTime? AdminResponseDeadline { get; set; }

    public DateTime? EscalatedAt { get; set; }

    public Guid? EscalatedToAdminId { get; set; }

    public DateTime? FirstResponseAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public DateTime? ReopenedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    public ApplicationUser SubmittedBy { get; set; } = null!;

    public ApplicationUser? AssignedToUser { get; set; }

    public ApplicationUser? ResolvedByUser { get; set; }

    public ApplicationUser? EscalatedToAdmin { get; set; }

    public Booking? Booking { get; set; }

    public MarketplaceOrder? MarketplaceOrder { get; set; }

    public ICollection<TicketMessage> Messages { get; set; } = new List<TicketMessage>();

}
