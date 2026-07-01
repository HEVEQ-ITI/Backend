using HEVEQ.Application.Features.Admin.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HEVEQ.Application.Features.Admin.Command.ReleaseBookingDispute
{
    public class ReleaseBookingDisputeCommand : IRequest<ReleaseBookingDisputeResponse>
    {
        [JsonIgnore] public Guid BookingId { get; set; }
        [JsonIgnore] public Guid AdminId { get; set; } // للتوثيق (Audit)

        public string DecisionNote { get; set; }
    }
}
