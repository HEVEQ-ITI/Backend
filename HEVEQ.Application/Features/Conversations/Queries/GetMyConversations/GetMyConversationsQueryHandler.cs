using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Conversations.DTOs;

namespace HEVEQ.Application.Features.Conversations.Queries.GetMyConversations;

public class GetMyConversationsQueryHandler
    : IRequestHandler<GetMyConversationsQuery, ConversationListResult>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetMyConversationsQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ConversationListResult> Handle(
        GetMyConversationsQuery request,
        CancellationToken cancellationToken)
    {
        // Business Rule: user sees conversations where they are a participant only
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        var items = await _context.Conversations
            .AsNoTracking()
            .Where(c => c.InitiatedById == userId || c.ParticipantId == userId)
            .OrderByDescending(c =>
                c.Messages.Any()
                    ? c.Messages.Max(m => m.SentAt)
                    : c.CreatedAt)
            .Select(c => new ConversationListItemDto
            {
                Id = c.Id,
                IsLocked = c.IsLocked,

                // ── Context type: only one of the three foreign keys is set ──
                ContextType =
                    c.BookingId.HasValue ? "Booking" :
                    c.ServiceListingId.HasValue ? "ServiceListing" :
                    c.MarketplaceListingId.HasValue ? "MarketplaceListing" :
                                                      "Direct",

                // ── Reference id: whichever context is set ────────────────────
                ReferenceId =
                    c.BookingId.HasValue ? c.BookingId :
                    c.ServiceListingId.HasValue ? c.ServiceListingId :
                    c.MarketplaceListingId.HasValue ? c.MarketplaceListingId :
                                                      (Guid?)null,

                // ── Title: prefer BookingNumber + listing title for clarity ────
                // Matches the spec example: "BK-1001 - CAT Excavator 320"
                Title =
                    c.Booking != null
                        ? (c.Booking.BookingNumber
                           + (c.Booking.ServiceListing != null
                               ? " - " + c.Booking.ServiceListing.Title
                               : string.Empty))
                    : c.ServiceListing != null
                        ? c.ServiceListing.Title
                    : c.MarketplaceListing != null
                        ? c.MarketplaceListing.Title
                    : "Conversation",

                // ── Other party name ──────────────────────────────────────────
                // Business Rule: never return phone/email — name only
                // If I initiated → the other party is Participant
                // If I'm the participant → the other party is the Initiator
                OtherPartyName = c.InitiatedById == userId
                    ? c.Participant.FirstName + " " + c.Participant.LastName
                    : c.InitiatedBy.FirstName + " " + c.InitiatedBy.LastName,

                // ── Last message preview ──────────────────────────────────────
                LastMessagePreview = c.Messages
                    .OrderByDescending(m => m.SentAt)
                    .Select(m => m.Content)
                    .FirstOrDefault(),

                LastMessageAt = c.Messages.Any()
                    ? c.Messages.Max(m => m.SentAt)
                    : (DateTime?)null,

                // ── Unread count ──────────────────────────────────────────────
                // Count messages not sent by me AND sent after my LastReadAt.
                // ConversationReadReceipts.LastReadAt is the stamp from PATCH /{id}/read.
                // If I have never marked this conversation as read → all messages
                // sent by the other party are unread.
                UnreadCount = c.Messages.Count(m =>
                    m.SenderId != userId &&
                    m.SentAt > (c.ReadReceipts
                        .Where(r => r.UserId == userId)
                        .Select(r => (DateTime?)r.LastReadAt)
                        .FirstOrDefault() ?? DateTime.MinValue))
            })
            .ToListAsync(cancellationToken);

        return new ConversationListResult
        {
            Items = items,
            TotalCount = items.Count
        };
    }
}