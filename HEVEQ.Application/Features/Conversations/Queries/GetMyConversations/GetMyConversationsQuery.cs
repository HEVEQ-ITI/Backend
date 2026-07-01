using HEVEQ.Application.Features.Conversations.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.Conversations.Queries.GetMyConversations;

public record GetMyConversationsQuery : IRequest<ConversationListResult>;