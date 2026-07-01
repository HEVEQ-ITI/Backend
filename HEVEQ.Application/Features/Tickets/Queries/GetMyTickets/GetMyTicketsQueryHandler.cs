using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Tickets.DTOs;
using HEVEQ.Application.Features.Tickets.Commands.CreateTicket;
using HEVEQ.Domain.Enums;

namespace HEVEQ.Application.Features.Tickets.Queries.GetMyTickets;

public class GetMyTicketsQueryHandler
    : IRequestHandler<GetMyTicketsQuery, MyTicketsResult>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetMyTicketsQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<MyTicketsResult> Handle(
        GetMyTicketsQuery request,
        CancellationToken cancellationToken)
    {
        // Business Rule: user sees only their own tickets
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        var tickets = await _context.Tickets
            .AsNoTracking()
            .Where(t => t.SubmittedById == userId)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TicketListItemDto
            {
                Id = t.Id,
                TicketNumber = t.TicketNumber,
                Subject = t.Subject,
                Status = t.Status.ToString(),
                StatusAr = CreateTicketCommandHandler.MapStatusAr(t.Status),
                CreatedAt = t.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return new MyTicketsResult
        {
            Items = tickets,
            TotalCount = tickets.Count
        };
    }
}