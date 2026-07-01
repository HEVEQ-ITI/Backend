using HEVEQ.Application.Features.Admin.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HEVEQ.Application.Features.Admin.Command.PartialSettleBookingDispute
{
    public class PartialSettleBookingDisputeCommand : IRequest<PartialSettleBookingDisputeResponse>
    {
        [JsonIgnore] public Guid BookingId { get; set; }
        [JsonIgnore] public Guid AdminId { get; set; }

        public decimal CustomerAmount { get; set; }
        public decimal ProviderAmount { get; set; }
        public string DecisionNote { get; set; }
    }
}
