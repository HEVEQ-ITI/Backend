using HEVEQ.Application.Features.Conversations.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;

namespace HEVEQ.Application.Features.Conversations.Commands.SendMessage;

public record SendMessageCommand(
    Guid ConversationId,
    string Body,
    MessageType MessageType
) : IRequest<SendMessageResult>;