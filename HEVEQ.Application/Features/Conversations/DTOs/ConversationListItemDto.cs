namespace HEVEQ.Application.Features.Conversations.DTOs;

public class ConversationListItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ContextType { get; set; } = string.Empty;
    public Guid? ReferenceId { get; set; }
    public string OtherPartyName { get; set; } = string.Empty;
    public string? LastMessagePreview { get; set; }
    public DateTime? LastMessageAt { get; set; }
    public int UnreadCount { get; set; }
    public bool IsLocked { get; set; }
}

// Wrapper returned by GET /api/conversations/my
public class ConversationListResult
{
    public List<ConversationListItemDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
}