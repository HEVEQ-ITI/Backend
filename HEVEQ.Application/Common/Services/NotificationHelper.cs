using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;

namespace HEVEQ.Application.Common.Services;

/// <summary>
/// Adds in-app Notification rows to the DbContext change tracker.
/// Must be injected as Scoped (same lifetime as DbContext).
///
/// Usage inside any command handler:
///     _notificationHelper.BookingAccepted(customerId, bookingId, bookingNumber);
///     await _context.SaveChangesAsync(cancellationToken);
///
/// The helper NEVER calls SaveChangesAsync itself.
/// The calling handler saves both the primary entity and the notification
/// in a single transaction — if anything fails, nothing is written.
///
/// In-app notifications only in this phase (Channel = InApp).
/// No FCM, no SMS, no email until those services are ready.
/// </summary>
public class NotificationHelper
{
    private readonly IApplicationDbContext _context;

    public NotificationHelper(IApplicationDbContext context)
    {
        _context = context;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Booking events
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Provider accepts a booking → notify the Customer.
    /// Called from: AcceptBookingCommandHandler
    /// </summary>
    public void BookingAccepted(Guid customerId, Guid bookingId, string bookingNumber)
        => Add(
            userId: customerId,
            eventType: "BookingAccepted",
            title: "Booking Accepted",
            body: $"Your booking {bookingNumber} has been accepted by the provider.",
            referenceId: bookingId.ToString(),
            referenceType: "Booking");

    /// <summary>
    /// Provider rejects a booking → notify the Customer.
    /// Called from: RejectBookingCommandHandler
    /// </summary>
    public void BookingRejected(Guid customerId, Guid bookingId, string bookingNumber)
        => Add(
            userId: customerId,
            eventType: "BookingRejected",
            title: "Booking Not Accepted",
            body: $"Your booking {bookingNumber} was not accepted by the provider.",
            referenceId: bookingId.ToString(),
            referenceType: "Booking");

    /// <summary>
    /// Booking marked as Completed → notify the Customer.
    /// Angular will show a prompt to leave a review after this notification.
    /// Called from: CompleteBookingCommandHandler
    /// </summary>
    public void BookingCompleted(Guid customerId, Guid bookingId, string bookingNumber)
        => Add(
            userId: customerId,
            eventType: "BookingCompleted",
            title: "Booking Completed",
            body: $"Your booking {bookingNumber} has been completed. You can now submit a review.",
            referenceId: bookingId.ToString(),
            referenceType: "Booking");

    // ─────────────────────────────────────────────────────────────────────────
    // Listing events
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Admin approves a service listing → notify the Provider.
    /// Called from: ApproveListingCommandHandler (Developer 1)
    /// </summary>
    public void ListingApproved(Guid providerUserId, Guid listingId, string listingTitle)
        => Add(
            userId: providerUserId,
            eventType: "ListingApproved",
            title: "Listing Approved",
            body: $"Your listing \"{listingTitle}\" has been approved and is now live.",
            referenceId: listingId.ToString(),
            referenceType: "ServiceListing");

    /// <summary>
    /// Admin rejects a service listing → notify the Provider.
    /// Called from: RejectListingCommandHandler (Developer 1)
    /// </summary>
    public void ListingRejected(Guid providerUserId, Guid listingId, string listingTitle)
        => Add(
            userId: providerUserId,
            eventType: "ListingRejected",
            title: "Listing Not Approved",
            body: $"Your listing \"{listingTitle}\" was not approved. Please review the admin notes and resubmit.",
            referenceId: listingId.ToString(),
            referenceType: "ServiceListing");

    // ─────────────────────────────────────────────────────────────────────────
    // Marketplace order events
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Seller marks order as dispatched → notify the Buyer.
    /// Called from: DispatchOrderCommandHandler
    /// </summary>
    public void OrderDispatched(Guid buyerId, Guid orderId)
        => Add(
            userId: buyerId,
            eventType: "OrderDispatched",
            title: "Order Dispatched",
            body: "Your order has been dispatched and is on its way to you.",
            referenceId: orderId.ToString(),
            referenceType: "MarketplaceOrder");

    // ─────────────────────────────────────────────────────────────────────────
    // Support ticket events
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Admin or support staff replies to a ticket → notify the ticket owner.
    /// Called from: AddAdminTicketMessageCommandHandler (Developer 1)
    /// </summary>
    public void TicketReplied(Guid ticketOwnerId, Guid ticketId, string ticketNumber)
        => Add(
            userId: ticketOwnerId,
            eventType: "TicketReplied",
            title: "Support Reply Received",
            body: $"A new reply has been added to your support ticket {ticketNumber}.",
            referenceId: ticketId.ToString(),
            referenceType: "Ticket");

    /// <summary>
    /// Admin resolves a ticket → notify the ticket owner.
    /// Called from: ResolveTicketCommandHandler (Developer 1)
    /// </summary>
    public void TicketResolved(Guid ticketOwnerId, Guid ticketId, string ticketNumber)
        => Add(
            userId: ticketOwnerId,
            eventType: "TicketResolved",
            title: "Ticket Resolved",
            body: $"Your support ticket {ticketNumber} has been resolved.",
            referenceId: ticketId.ToString(),
            referenceType: "Ticket");

    // ─────────────────────────────────────────────────────────────────────────
    // Dispute events
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Admin resolves a dispute → notify one party.
    /// Call this TWICE — once for each party (customer and provider).
    /// Called from: ResolveDisputeCommandHandler (Developer 1)
    /// </summary>
    public void DisputeResolved(
        Guid userId,
        Guid bookingId,
        string bookingNumber,
        string outcome)
        => Add(
            userId: userId,
            eventType: "DisputeResolved",
            title: "Dispute Resolved",
            body: $"The dispute for booking {bookingNumber} has been resolved. Outcome: {outcome}.",
            referenceId: bookingId.ToString(),
            referenceType: "Booking");

    // ─────────────────────────────────────────────────────────────────────────
    // Private helper — single point of Notification construction
    // ─────────────────────────────────────────────────────────────────────────

    private void Add(
        Guid userId,
        string eventType,
        string title,
        string body,
        string? referenceId = null,
        string? referenceType = null)
    {
        _context.Notifications.Add(new Notification
        {
            // Id auto-generated by Guid.NewGuid() default in the entity
            UserId = userId,
            EventType = eventType,
            Title = title,
            Body = body,
            ReferenceId = referenceId,
            ReferenceType = referenceType,
            IsRead = false,
            Channel = NotificationChannel.InApp,  // in-app only this phase
            SentAt = DateTime.UtcNow,
            ReadAt = null
        });

        //  SaveChangesAsync is NOT called here.
        // The calling handler saves everything in one transaction.
    }
}