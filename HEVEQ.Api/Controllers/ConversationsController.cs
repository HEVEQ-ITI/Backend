using HEVEQ.Application.Features.Conversations.Commands.MarkConversationRead;
using HEVEQ.Application.Features.Conversations.Commands.SendMessage;
using HEVEQ.Application.Features.Conversations.Commands.StartConversation;
using HEVEQ.Application.Features.Conversations.Queries.GetConversationMessages;
using HEVEQ.Application.Features.Conversations.Queries.GetMyConversations;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HEVEQ.Api.Controllers;

[ApiController]
[Route("api/conversations")]
[Authorize]
public class ConversationsController : ControllerBase
{
    private readonly ISender _mediator;

    public ConversationsController(ISender mediator)
    {
        _mediator = mediator;
    }

    // ── GET /api/conversations/my ─────────────────────────────────────────────
    // Returns all conversations where the current user is InitiatedBy or Participant.
    // Ordered by last message descending.
    // No contact info (phone/email) in the response — names only.
    [HttpGet("my")]
    public async Task<IActionResult> GetMy(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMyConversationsQuery(), cancellationToken);
        return Ok(result);
    }

    // ── POST /api/conversations ───────────────────────────────────────────────
    // Start a conversation for a Booking, ServiceListing, or MarketplaceListing.
    // Idempotent: returns the existing conversation id if one already exists
    // for the same context + same pair of users.
    [HttpPost]
    public async Task<IActionResult> Start(
        [FromBody] StartConversationCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Created($"api/conversations/{result.Id}/messages", result);
    }

    // ── GET /api/conversations/{id}/messages?page=1&pageSize=20 ──────────────
    // Paginated message history. Messages ordered oldest-first (chat view).
    // User must be a participant.
    [HttpGet("{id:guid}/messages")]
    public async Task<IActionResult> GetMessages(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        if (page < 1) page = 1;
        if (pageSize is < 1 or > 100) pageSize = 20;

        var result = await _mediator.Send(
            new GetConversationMessagesQuery(id, page, pageSize),
            cancellationToken);

        return Ok(result);
    }

    // ── POST /api/conversations/{id}/messages ─────────────────────────────────
    // Send a message. Contact info in body is blocked by validator.
    // Locked conversations return 400.
    // id comes from the route — body only carries content and type.
    [HttpPost("{id:guid}/messages")]
    public async Task<IActionResult> SendMessage(
        Guid id,
        [FromBody] SendMessageBody body,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new SendMessageCommand(id, body.Body, body.MessageType ?? MessageType.Text),
            cancellationToken);

        return Created(string.Empty, result);
    }

    // ── PATCH /api/conversations/{id}/read ───────────────────────────────────
    // Stamps a ConversationReadReceipt to the latest message.
    // Sets unreadCount = 0 for this conversation for the current user.
    // Idempotent — calling twice is safe.
    [HttpPatch("{id:guid}/read")]
    public async Task<IActionResult> MarkRead(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new MarkConversationReadCommand(id),
            cancellationToken);

        return Ok(result);
    }
}

// Thin body record — conversation id comes from the route path, not the body
public record SendMessageBody(string Body, MessageType? MessageType);