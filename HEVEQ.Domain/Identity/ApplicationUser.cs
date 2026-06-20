using HEVEQ.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace HEVEQ.Domain.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public string? FcmToken { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Address> Addresses { get; set; } = new List<Address>();

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public ProviderProfile? ProviderProfile { get; set; }

    public CustomerProfile? CustomerProfile { get; set; }

    public EmployeeProfile? EmployeeProfile { get; set; }

    public ICollection<Document> Documents { get; set; } = new List<Document>();

    public ICollection<Booking> BookingsAsCustomer { get; set; } = new List<Booking>();

    public ICollection<MarketplaceListing> MarketplaceListingsAsSeller { get; set; } = new List<MarketplaceListing>();

    public ICollection<MarketplaceOrder> MarketplaceOrdersAsBuyer { get; set; } = new List<MarketplaceOrder>();

    public ICollection<Review> ReviewsWritten { get; set; } = new List<Review>();

    public ICollection<Review> ReviewsReceived { get; set; } = new List<Review>();

    public ICollection<Ticket> SubmittedTickets { get; set; } = new List<Ticket>();

    public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();

    public ICollection<Ticket> ResolvedTickets { get; set; } = new List<Ticket>();

    public ICollection<Ticket> EscalatedTickets { get; set; } = new List<Ticket>();

    public ICollection<TicketMessage> TicketMessages { get; set; } = new List<TicketMessage>();

    public ICollection<TicketAttachment> TicketAttachments { get; set; } = new List<TicketAttachment>();

    public ICollection<JobCompletionEvidenceForm> JobCompletionEvidenceFormsSubmitted { get; set; } = new List<JobCompletionEvidenceForm>();

    public ICollection<JobCompletionEvidenceForm> JobCompletionEvidenceFormsReviewed { get; set; } = new List<JobCompletionEvidenceForm>();

    public ICollection<FieldVerificationForm> FieldVerificationFormsDispatched { get; set; } = new List<FieldVerificationForm>();

    public ICollection<FieldVerificationForm> FieldVerificationFormsDispatchedByAdmin { get; set; } = new List<FieldVerificationForm>();

    public ICollection<FieldVerificationForm> FieldVerificationFormsDecidedByAdmin { get; set; } = new List<FieldVerificationForm>();

    public ICollection<Conversation> ConversationsInitiated { get; set; } = new List<Conversation>();

    public ICollection<Conversation> ConversationsParticipated { get; set; } = new List<Conversation>();

    public ICollection<Message> Messages { get; set; } = new List<Message>();

    public ICollection<ConversationReadReceipt> ConversationReadReceipts { get; set; } = new List<ConversationReadReceipt>();

    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public ICollection<SearchQueryLog> SearchQueryLogs { get; set; } = new List<SearchQueryLog>();

    public ICollection<PlatformSetting> PlatformSettingsUpdated { get; set; } = new List<PlatformSetting>();

    public ICollection<AiInteractionLog> AiInteractionLogsAdminOverrides { get; set; } = new List<AiInteractionLog>();
}
