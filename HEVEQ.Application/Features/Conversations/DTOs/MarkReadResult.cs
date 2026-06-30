namespace HEVEQ.Application.Features.Conversations.DTOs;

// Returned by PATCH /api/conversations/{id}/read
// UnreadCount is always 0 after marking read —
// Angular uses this to immediately update the badge without a second request.
public record MarkReadResult(Guid ConversationId, int UnreadCount, string Message);