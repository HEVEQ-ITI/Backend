using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Tickets.Commands.CreateTicket;
using HEVEQ.Application.Features.Tickets.DTOs;

namespace HEVEQ.Application.Features.Tickets.Queries.GetTicketDetails;

public class GetTicketDetailsQueryHandler
    : IRequestHandler<GetTicketDetailsQuery, TicketDetailsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetTicketDetailsQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<TicketDetailsDto> Handle(
        GetTicketDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        // Load ticket with its messages and each message's sender name
        var ticket = await _context.Tickets
            .AsNoTracking()
            .Include(t => t.Messages.Where(m => !m.IsInternal)) // exclude internal notes at DB level
                .ThenInclude(m => m.Sender)
            .FirstOrDefaultAsync(t => t.Id == request.TicketId, cancellationToken)
            ?? throw new NotFoundException("Ticket", request.TicketId);

        // Business Rule: user can only view their own ticket
        if (ticket.SubmittedById != userId)
            throw new ForbiddenAccessException(
                "You do not have access to this ticket.");

        return new TicketDetailsDto
        {
            Id = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            Subject = ticket.Subject,
            Status = ticket.Status.ToString(),
            StatusAr = CreateTicketCommandHandler.MapStatusAr(ticket.Status),
            // Messages are already filtered to IsInternal=false by the Include filter above
            // Sorted oldest first so the conversation reads top-to-bottom
            Messages = ticket.Messages
                .OrderBy(m => m.CreatedAt)
                .Select(m => new TicketMessageDto
                {
                    Id = m.Id,
                    SenderName = m.Sender.FirstName + " " + m.Sender.LastName,
                    Body = m.Body,
                    CreatedAt = m.CreatedAt
                })
                .ToList()
        };
    }
}