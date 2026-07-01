using HEVEQ.Application.Features.Admin.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HEVEQ.Application.Features.Admin.Command.ResolveTicket
{
    public class ResolveTicketCommand : IRequest<ResolveTicketResponse>
    {
        [JsonIgnore] public Guid Id { get; set; }
        public TicketResolutionType ResolutionType { get; set; } 
        public string AdminResolution { get; set; } 
    }
}
