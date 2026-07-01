using HEVEQ.Application.Features.Admin.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HEVEQ.Application.Features.Admin.Command.RejectMarketplaceListing
{
    public class RejectMarketplaceListingCommand : IRequest<RejectMarketplaceListingResponse>
    {
        [JsonIgnore] public Guid Id { get; set; }
        [JsonIgnore] public Guid AdminId { get; set; }

        public string AdminRejectionNote { get; set; }
    }
}
