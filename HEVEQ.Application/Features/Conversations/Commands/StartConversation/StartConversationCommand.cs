using HEVEQ.Application.Features.Conversations.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.Conversations.Commands.StartConversation;

public record StartConversationCommand(
    string ContextType,   
    Guid ReferenceId
) : IRequest<StartConversationResult>;