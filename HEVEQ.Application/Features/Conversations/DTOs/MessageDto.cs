namespace HEVEQ.Application.Features.Conversations.DTOs;

public class MessageDto
{
    public Guid Id { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string MessageType { get; set; } = string.Empty;
    public string? Body { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Wrapper returned by GET /api/conversations/{id}/messages
public class ConversationMessagesResult
{
    public Guid ConversationId { get; set; }
    public List<MessageDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
}