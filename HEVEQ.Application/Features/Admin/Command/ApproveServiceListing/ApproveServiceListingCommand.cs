using HEVEQ.Application.Features.Admin.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HEVEQ.Application.Features.Admin.Command.ApproveServiceListing
{
    public class ApproveServiceListingCommand : IRequest<ApproveServiceListingResponse>
    {
        [JsonIgnore] 
        public Guid Id { get; set; }
    }
}
