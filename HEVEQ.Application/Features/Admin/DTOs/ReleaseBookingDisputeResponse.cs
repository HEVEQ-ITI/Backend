using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HEVEQ.Application.Features.Admin.DTOs
{
    public class ReleaseBookingDisputeResponse
    {
        public Guid BookingId { get; set; }
        public string Status { get; set; }
        public string StatusAr { get; set; }
        public string EscrowStatus { get; set; }
        public string Message { get; set; }

        [JsonIgnore] public bool IsSuccess { get; set; }
        [JsonIgnore] public int StatusCode { get; set; }
    }
}
