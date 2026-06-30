using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Tickets.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;

namespace HEVEQ.Application.Features.Tickets.Commands.AddTicketMessage;

public class AddTicketMessageCommandHandler
    : IRequestHandler<AddTicketMessageCommand, AddTicketMessageResult>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public AddTicketMessageCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<AddTicketMessageResult> Handle(
        AddTicketMessageCommand request,
        CancellationToken cancellationToken)
    {
        // Business Rule: authenticated only
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.Id == request.TicketId, cancellationToken)
            ?? throw new NotFoundException("Ticket", request.TicketId);

        // Business Rule: user can only add messages to their own ticket
        if (ticket.SubmittedById != userId)
            throw new ForbiddenAccessException(
                "You do not have access to this ticket.");

        // Business Rule: Resolved and Closed tickets cannot receive new messages
        if (ticket.Status is TicketStatus.Resolved or TicketStatus.Closed)
            throw new InvalidOperationException(
                "Cannot add a message to a resolved or closed ticket. Please open a new ticket.");

        var message = new TicketMessage
        {
            TicketId = ticket.Id,
            SenderId = userId,
            Body = request.Body,
            IsInternal = false,           // user messages are never internal
            CreatedAt = DateTime.UtcNow
        };

        _context.TicketMessages.Add(message);

        // Auto-advance ticket status: if admin was waiting for the user's reply,
        // the ticket moves back to InProgress when user replies
        if (ticket.Status == TicketStatus.PendingCustomerReply)
        {
            ticket.Status = TicketStatus.InProgress;
            ticket.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new AddTicketMessageResult(message.Id, "Message added successfully");
    }
}