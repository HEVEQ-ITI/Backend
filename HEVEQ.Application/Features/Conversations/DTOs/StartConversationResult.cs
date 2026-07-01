namespace HEVEQ.Application.Features.Conversations.DTOs;

// Returned by POST /api/conversations
// Idempotent — if a conversation already exists for the same context,
// the existing Id is returned with the same message.
public record StartConversationResult(Guid Id, string Message);