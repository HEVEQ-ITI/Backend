using HEVEQ.Application.Features.Tickets.Commands.AddTicketMessage;
using HEVEQ.Application.Features.Tickets.Commands.CreateTicket;
using HEVEQ.Application.Features.Tickets.Queries.GetMyTickets;
using HEVEQ.Application.Features.Tickets.Queries.GetTicketDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HEVEQ.Api.Controllers;

[ApiController]
[Route("api/tickets")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly ISender _mediator;

    public TicketsController(ISender mediator)
    {
        _mediator = mediator;
    }

    // ── POST /api/tickets ─────────────────────────────────────────────────────
    // Creates a support ticket + first TicketMessage.
    // Ticket number is auto-generated: TKT-0001, TKT-0002 ...
    // Links to booking OR marketplace order — never both at once.
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTicketCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Created(string.Empty, result);
    }

    // ── GET /api/tickets/my ───────────────────────────────────────────────────
    // Returns the current user's own tickets only — ordered newest first.
    [HttpGet("my")]
    public async Task<IActionResult> GetMy(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMyTicketsQuery(), cancellationToken);
        return Ok(result);
    }

    // ── GET /api/tickets/{id} ─────────────────────────────────────────────────
    // Returns full ticket details including messages.
    // Internal admin notes (IsInternal=true) are never returned to the user.
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetDetails(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTicketDetailsQuery(id), cancellationToken);
        return Ok(result);
    }

    // ── POST /api/tickets/{id}/messages ──────────────────────────────────────
    // User adds a reply to their own open/in-progress ticket.
    // Blocked for Resolved or Closed tickets.
    // id comes from the route — body only carries the message text.
    [HttpPost("{id:guid}/messages")]
    public async Task<IActionResult> AddMessage(
        Guid id,
        [FromBody] AddTicketMessageBody body,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new AddTicketMessageCommand(id, body.Body),
            cancellationToken);
        return Created(string.Empty, result);
    }
}

// Thin request body — ticket id comes from route, not body (general rule #2)
public record AddTicketMessageBody(string Body);