using HEVEQ.Application.Features.Tickets.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.Tickets.Commands.AddTicketMessage;

public record AddTicketMessageCommand(
    Guid TicketId,
    string Body
) : IRequest<AddTicketMessageResult>;