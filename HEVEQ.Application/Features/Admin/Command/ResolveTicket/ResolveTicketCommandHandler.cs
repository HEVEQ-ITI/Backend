using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Admin.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.ResolveTicket
{
    public class ResolveTicketCommandHandler(IApplicationDbContext context)
        : IRequestHandler<ResolveTicketCommand, ResolveTicketResponse>
    {
        public async Task<ResolveTicketResponse> Handle(ResolveTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = await context.Tickets
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (ticket == null)
                return new ResolveTicketResponse { IsSuccess = false, StatusCode = 404, Message = "Ticket not found." };

            if (ticket.Status == TicketStatus.Resolved || ticket.Status == TicketStatus.Closed)
                return new ResolveTicketResponse { IsSuccess = false, StatusCode = 400, Message = "Ticket is already resolved or closed." };

            ticket.Status = TicketStatus.Resolved;
            ticket.AdminResolution = request.AdminResolution;
            ticket.ResolutionType = request.ResolutionType;
            ticket.ResolvedAt = DateTime.UtcNow; 
            ticket.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);

            return new ResolveTicketResponse
            {
                IsSuccess = true,
                StatusCode = 200,
                Id = ticket.Id,
                Status = "Resolved",
                StatusAr = "تم الحل",
                Message = "Ticket resolved successfully"
            };
        }
    }
}
