namespace HEVEQ.Application.Features.Conversations.DTOs;

// Returned by POST /api/conversations/{id}/messages
public record SendMessageResult(Guid Id, string Message);