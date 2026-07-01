using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Tickets.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;

namespace HEVEQ.Application.Features.Tickets.Commands.CreateTicket;

public class CreateTicketCommandHandler
    : IRequestHandler<CreateTicketCommand, CreateTicketResult>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateTicketCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<CreateTicketResult> Handle(
        CreateTicketCommand request,
        CancellationToken cancellationToken)
    {
        // Business Rule: authenticated only
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        // Generate sequential ticket number: TKT-0001, TKT-0002 ...
        // Count all tickets to derive next number — safe for low volume in this phase
        var count = await _context.Tickets.CountAsync(cancellationToken);
        var ticketNumber = $"TKT-{(count + 1):D4}";

        var ticket = new Ticket
        {
            SubmittedById = userId,
            TicketNumber = ticketNumber,
            Subject = request.Subject,
            Category = request.Category,
            BookingId = request.BookingId,
            MarketplaceOrderId = request.MarketplaceOrderId,
            Status = TicketStatus.Open,
            Priority = 1,           // default — admin escalates if needed
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Tickets.Add(ticket);

        // Full Flow Step 4: create the first TicketMessage from the user's initial message
        var firstMessage = new TicketMessage
        {
            TicketId = ticket.Id,
            SenderId = userId,
            Body = request.Message,
            IsInternal = false,   // always false for user-submitted messages
            CreatedAt = DateTime.UtcNow
        };

        _context.TicketMessages.Add(firstMessage);
        await _context.SaveChangesAsync(cancellationToken);

        // Full Flow Step 5: ticket now appears in admin queue (Developer 1's side)
        return new CreateTicketResult
        {
            Id = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            Status = ticket.Status.ToString(),
            StatusAr = MapStatusAr(ticket.Status),
            Message = "Ticket created successfully"
        };
    }

    internal static string MapStatusAr(TicketStatus status) => status switch
    {
        TicketStatus.Open => "مفتوحة",
        TicketStatus.InProgress => "قيد المعالجة",
        TicketStatus.PendingCustomerReply => "بانتظار ردك",
        TicketStatus.PendingProviderReply => "بانتظار رد المزود",
        TicketStatus.PendingFieldVerification => "بانتظار التحقق الميداني",
        TicketStatus.Resolved => "محلولة",
        TicketStatus.Closed => "مغلقة",
        TicketStatus.Reopened => "معاد فتحها",
        _ => status.ToString()
    };
}