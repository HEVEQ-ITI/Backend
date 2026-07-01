using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Conversations.DTOs;
using HEVEQ.Domain.Entities;

namespace HEVEQ.Application.Features.Conversations.Commands.MarkConversationRead;

public class MarkConversationReadCommandHandler
    : IRequestHandler<MarkConversationReadCommand, MarkReadResult>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public MarkConversationReadCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<MarkReadResult> Handle(
        MarkConversationReadCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        // Verify conversation exists and user is a participant
        var conversation = await _context.Conversations
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Conversation", request.ConversationId);

        if (conversation.InitiatedById != userId && conversation.ParticipantId != userId)
            throw new ForbiddenAccessException(
                "You are not a participant in this conversation.");

        // Find the latest message to stamp as the read point
        var lastMessage = await _context.Messages
            .AsNoTracking()
            .Where(m => m.ConversationId == request.ConversationId)
            .OrderByDescending(m => m.SentAt)
            .Select(m => new { m.Id, m.SentAt })
            .FirstOrDefaultAsync(cancellationToken);

        // Business Rule: upsert ConversationReadReceipt for current user
        // ConversationReadReceipt PK is (ConversationId, UserId) — composite key
        var receipt = await _context.ConversationReadReceipts
            .FirstOrDefaultAsync(r =>
                r.ConversationId == request.ConversationId &&
                r.UserId == userId,
                cancellationToken);

        if (receipt is null)
        {
            // First time this user marks this conversation as read
            _context.ConversationReadReceipts.Add(new ConversationReadReceipt
            {
                ConversationId = request.ConversationId,
                UserId = userId,
                LastReadMessageId = lastMessage?.Id,
                LastReadAt = DateTime.UtcNow
            });
        }
        else
        {
            // Update existing receipt — stamp advances to latest message
            receipt.LastReadMessageId = lastMessage?.Id;
            receipt.LastReadAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);

        // After marking read, unread count for this conversation is 0
        return new MarkReadResult(request.ConversationId, 0, "Conversation marked as read");
    }
}