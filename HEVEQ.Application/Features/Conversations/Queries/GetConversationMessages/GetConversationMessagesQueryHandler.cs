using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Conversations.DTOs;

namespace HEVEQ.Application.Features.Conversations.Queries.GetConversationMessages;

public class GetConversationMessagesQueryHandler
    : IRequestHandler<GetConversationMessagesQuery, ConversationMessagesResult>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetConversationMessagesQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ConversationMessagesResult> Handle(
        GetConversationMessagesQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        // Business Rule: user must be a participant
        var conversation = await _context.Conversations
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Conversation", request.ConversationId);

        if (conversation.InitiatedById != userId && conversation.ParticipantId != userId)
            throw new ForbiddenAccessException(
                "You are not a participant in this conversation.");

        // Pagination: get total before applying Skip/Take
        var totalCount = await _context.Messages
            .AsNoTracking()
            .CountAsync(m => m.ConversationId == request.ConversationId, cancellationToken);

        // Load messages oldest-first so the chat reads top-to-bottom
        var items = await _context.Messages
            .AsNoTracking()
            .Where(m => m.ConversationId == request.ConversationId)
            .OrderBy(m => m.SentAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(m => new MessageDto
            {
                Id = m.Id,
                SenderName = m.Sender.FirstName + " " + m.Sender.LastName,
                MessageType = m.MessageType.ToString(),   // "Text" | "Attachment" | "System"
                Body = m.Content,
                CreatedAt = m.SentAt
            })
            .ToListAsync(cancellationToken);

        return new ConversationMessagesResult
        {
            ConversationId = request.ConversationId,
            Items = items,
            TotalCount = totalCount
        };
    }
}