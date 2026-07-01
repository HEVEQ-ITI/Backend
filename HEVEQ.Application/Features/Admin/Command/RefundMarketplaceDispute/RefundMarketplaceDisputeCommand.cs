using HEVEQ.Application.Features.Admin.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HEVEQ.Application.Features.Admin.Command.RefundMarketplaceDispute
{
    public class RefundMarketplaceDisputeCommand : IRequest<RefundMarketplaceDisputeResponse>
    {
        [JsonIgnore] public Guid OrderId { get; set; }
        [JsonIgnore] public Guid AdminId { get; set; } 
        public string DecisionNote { get; set; }
    }
}
