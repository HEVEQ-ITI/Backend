using HEVEQ.Application.Features.Conversations.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.Conversations.Queries.GetConversationMessages;

public record GetConversationMessagesQuery(
    Guid ConversationId,
    int Page,
    int PageSize
) : IRequest<ConversationMessagesResult>;