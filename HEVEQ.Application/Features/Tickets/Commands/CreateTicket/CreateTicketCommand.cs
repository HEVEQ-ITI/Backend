using HEVEQ.Application.Features.Tickets.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;

namespace HEVEQ.Application.Features.Tickets.Commands.CreateTicket;

public record CreateTicketCommand(
    string Subject,
    TicketCategory Category,
    string Message,
    Guid? BookingId,
    Guid? MarketplaceOrderId
) : IRequest<CreateTicketResult>;