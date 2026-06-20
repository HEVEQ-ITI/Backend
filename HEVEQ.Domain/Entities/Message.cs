using HEVEQ.Domain.Enums;
using HEVEQ.Domain.Identity;

namespace HEVEQ.Domain.Entities;

public class Message
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ConversationId { get; set; }

    public Guid SenderId { get; set; }

    public string? Content { get; set; }

    public MessageType MessageType { get; set; }

    public string? AttachmentUrl { get; set; }

    public bool IsBlocked { get; set; }

    public DateTime SentAt { get; set; }
    public Conversation Conversation { get; set; } = null!;

    public ApplicationUser Sender { get; set; } = null!;

    public ICollection<ConversationReadReceipt> ReadReceiptsAsLastRead { get; set; } = new List<ConversationReadReceipt>();

}
