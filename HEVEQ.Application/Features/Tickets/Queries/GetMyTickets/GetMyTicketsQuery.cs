using HEVEQ.Application.Features.Tickets.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.Tickets.Queries.GetMyTickets;

public record GetMyTicketsQuery : IRequest<MyTicketsResult>;