using HEVEQ.Application.Features.Tickets.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.Tickets.Queries.GetTicketDetails;

public record GetTicketDetailsQuery(Guid TicketId) : IRequest<TicketDetailsDto>;