using HEVEQ.Domain.Identity;
namespace HEVEQ.Domain.Entities;

public class ConversationReadReceipt
{
    public Guid ConversationId { get; set; }

    public Guid UserId { get; set; }

    public Guid? LastReadMessageId { get; set; }

    public DateTime LastReadAt { get; set; }
    public Conversation Conversation { get; set; } = null!;

    public ApplicationUser User { get; set; } = null!;

    public Message? LastReadMessage { get; set; }

}
