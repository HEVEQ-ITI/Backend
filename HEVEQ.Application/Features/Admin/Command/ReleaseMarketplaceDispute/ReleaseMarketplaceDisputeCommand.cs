using HEVEQ.Application.Features.Admin.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HEVEQ.Application.Features.Admin.Command.ReleaseMarketplaceDispute
{
    public class ReleaseMarketplaceDisputeCommand : IRequest<ReleaseMarketplaceDisputeResponse>
    {
        [JsonIgnore] public Guid OrderId { get; set; }
        [JsonIgnore] public Guid AdminId { get; set; } 

        public string DecisionNote { get; set; }
    }
}
