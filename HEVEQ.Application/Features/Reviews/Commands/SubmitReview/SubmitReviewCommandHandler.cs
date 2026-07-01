using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Reviews.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;

namespace HEVEQ.Application.Features.Reviews.Commands.SubmitReview;

public class SubmitReviewCommandHandler
    : IRequestHandler<SubmitReviewCommand, SubmitReviewResult>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public SubmitReviewCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<SubmitReviewResult> Handle(
        SubmitReviewCommand request,
        CancellationToken cancellationToken)
    {
        var reviewerId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        Guid reviewedUserId;

        if (request.BookingId.HasValue)
        {
            // ── Booking path ──────────────────────────────────────────────────

            var booking = await _context.Bookings
                .AsNoTracking()
                .Include(b => b.ServiceListing)
                    .ThenInclude(l => l.ProviderProfile)
                .FirstOrDefaultAsync(b => b.Id == request.BookingId.Value, cancellationToken)
                ?? throw new NotFoundException(nameof(Booking), request.BookingId.Value);

            // Business Rule: review only allowed after Completed
            if (booking.Status != BookingStatus.Completed)
                throw new InvalidOperationException(
                    "A review can only be submitted after the booking is completed.");

            // Business Rule: reviewer must be the customer of this booking
            if (booking.CustomerId != reviewerId)
                throw new ForbiddenAccessException(
                    "You are not the customer of this booking.");

            // Business Rule: no duplicate review from same user on same booking
            var alreadyReviewed = await _context.Reviews
                .AnyAsync(r => r.ReviewerId == reviewerId
                               && r.BookingId == request.BookingId.Value,
                          cancellationToken);

            if (alreadyReviewed)
                throw new InvalidOperationException(
                    "You have already submitted a review for this booking.");

            // Reviewed user = the provider's ApplicationUser
            reviewedUserId = booking.ServiceListing.ProviderProfile.UserId;
        }
        else
        {
            // ── Marketplace order path ────────────────────────────────────────

            var order = await _context.MarketplaceOrders
                .AsNoTracking()
                .Include(o => o.Listing)
                .FirstOrDefaultAsync(o => o.Id == request.MarketplaceOrderId!.Value, cancellationToken)
                ?? throw new NotFoundException(nameof(MarketplaceOrder), request.MarketplaceOrderId!.Value);

            // Business Rule: review only allowed after Completed
            if (order.Status != MarketplaceOrderStatus.Completed)
                throw new InvalidOperationException(
                    "A review can only be submitted after the order is completed.");

            // Business Rule: reviewer must be the buyer of this order
            if (order.BuyerId != reviewerId)
                throw new ForbiddenAccessException(
                    "You are not the buyer of this order.");

            // Business Rule: no duplicate review from same user on same order
            var alreadyReviewed = await _context.Reviews
                .AnyAsync(r => r.ReviewerId == reviewerId
                               && r.MarketplaceOrderId == request.MarketplaceOrderId!.Value,
                          cancellationToken);

            if (alreadyReviewed)
                throw new InvalidOperationException(
                    "You have already submitted a review for this order.");

            // Reviewed user = the seller of the marketplace listing
            reviewedUserId = order.Listing.SellerId;
        }

        // ── Create Review ─────────────────────────────────────────────────────
        // No AI moderation in this phase — auto-publish immediately.
        // ModerationStatus stays Pending for future AI phase to pick up.
        var review = new Review
        {
            ReviewerId = reviewerId,
            ReviewedUserId = reviewedUserId,
            BookingId = request.BookingId,
            MarketplaceOrderId = request.MarketplaceOrderId,
            Rating = request.Rating,
            Comment = request.Comment,
            ModerationStatus = ModerationStatus.Pending,
            IsPublished = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Reviews.Add(review);

        // ── Step 6: Update rating aggregates on the reviewed user's profile ───
        // Provider: update AverageRating + TotalReviewsCount
        // Seller: update AverageRating on their MarketplaceListing (if the listing tracks it)
        if (request.BookingId.HasValue)
        {
            var providerProfile = await _context.ProviderProfiles
                .FirstOrDefaultAsync(p => p.UserId == reviewedUserId, cancellationToken);

            if (providerProfile is not null)
            {
                // Recalculate average from DB for accuracy
                var existingRatings = await _context.Reviews
                    .Where(r => r.ReviewedUserId == reviewedUserId && r.IsPublished)
                    .Select(r => r.Rating)
                    .ToListAsync(cancellationToken);

                existingRatings.Add(request.Rating);  

                providerProfile.TotalReviewsCount = existingRatings.Count;
                providerProfile.AverageRating = Math.Round(
                    (decimal)existingRatings.Average(), 2);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new SubmitReviewResult
        {
            Id = review.Id,
            Rating = review.Rating,
            IsPublished = review.IsPublished,
            Message = "Review submitted successfully"
        };
    }
}