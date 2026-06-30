using HEVEQ.Application.Features.Conversations.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.Conversations.Commands.MarkConversationRead;

public record MarkConversationReadCommand(Guid ConversationId) : IRequest<MarkReadResult>;