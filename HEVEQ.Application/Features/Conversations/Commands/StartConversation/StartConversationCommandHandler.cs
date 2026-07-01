using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Conversations.DTOs;
using HEVEQ.Domain.Entities;

namespace HEVEQ.Application.Features.Conversations.Commands.StartConversation;

public class StartConversationCommandHandler
    : IRequestHandler<StartConversationCommand, StartConversationResult>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public StartConversationCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<StartConversationResult> Handle(
        StartConversationCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        // ── Resolve context, verify user is a party, identify other party ─────
        Guid? bookingId = null;
        Guid? serviceListingId = null;
        Guid? marketplaceListingId = null;
        Guid otherPartyId;

        switch (request.ContextType)
        {
            case "Booking":
                {
                    var booking = await _context.Bookings
                        .AsNoTracking()
                        .Include(b => b.ServiceListing)
                            .ThenInclude(l => l.ProviderProfile)
                        .FirstOrDefaultAsync(b => b.Id == request.ReferenceId, cancellationToken)
                        ?? throw new NotFoundException("Booking", request.ReferenceId);

                    var providerUserId = booking.ServiceListing.ProviderProfile.UserId;

                    // Business Rule: user must be the customer OR the provider of this booking
                    if (booking.CustomerId != userId && providerUserId != userId)
                        throw new ForbiddenAccessException(
                            "You are not a party to this booking.");

                    bookingId = request.ReferenceId;
                    otherPartyId = booking.CustomerId == userId ? providerUserId : booking.CustomerId;
                    break;
                }

            case "ServiceListing":
                {
                    var listing = await _context.ServiceListings
                        .AsNoTracking()
                        .Include(l => l.ProviderProfile)
                        .FirstOrDefaultAsync(l => l.Id == request.ReferenceId, cancellationToken)
                        ?? throw new NotFoundException("ServiceListing", request.ReferenceId);

                    serviceListingId = request.ReferenceId;
                    otherPartyId = listing.ProviderProfile.UserId;

                    // Business Rule: cannot start a conversation with yourself
                    if (otherPartyId == userId)
                        throw new BadRequestException(
                            "You cannot start a conversation with yourself.");
                    break;
                }

            case "MarketplaceListing":
                {
                    var listing = await _context.MarketplaceListings
                        .AsNoTracking()
                        .FirstOrDefaultAsync(l => l.Id == request.ReferenceId, cancellationToken)
                        ?? throw new NotFoundException("MarketplaceListing", request.ReferenceId);

                    marketplaceListingId = request.ReferenceId;
                    otherPartyId = listing.SellerId;

                    if (otherPartyId == userId)
                        throw new BadRequestException(
                            "You cannot start a conversation with yourself.");
                    break;
                }

            default:
                throw new BadRequestException("Invalid ContextType.");
        }

        // ── Business Rule: no duplicate conversation for same context ─────────
        // Check if a conversation already exists between these two users
        // for the exact same booking / listing
        var existing = await _context.Conversations
            .AsNoTracking()
            .FirstOrDefaultAsync(c =>
                ((c.InitiatedById == userId && c.ParticipantId == otherPartyId) ||
                 (c.InitiatedById == otherPartyId && c.ParticipantId == userId)) &&
                c.BookingId == bookingId &&
                c.ServiceListingId == serviceListingId &&
                c.MarketplaceListingId == marketplaceListingId,
                cancellationToken);

        // Idempotent: return existing conversation instead of creating a duplicate
        if (existing is not null)
            return new StartConversationResult(existing.Id, "Conversation ready");

        // ── Create new conversation ───────────────────────────────────────────
        var conversation = new Conversation
        {
            InitiatedById = userId,
            ParticipantId = otherPartyId,
            BookingId = bookingId,
            ServiceListingId = serviceListingId,
            MarketplaceListingId = marketplaceListingId,
            IsLocked = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync(cancellationToken);

        return new StartConversationResult(conversation.Id, "Conversation ready");
    }
}