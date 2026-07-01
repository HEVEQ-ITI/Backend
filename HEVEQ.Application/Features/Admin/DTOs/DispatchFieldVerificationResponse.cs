using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HEVEQ.Application.Features.Admin.DTOs
{
    public class DispatchFieldVerificationResponse
    {
        public Guid FieldVerificationFormId { get; set; }
        public Guid BookingId { get; set; }
        public string VisitStatus { get; set; }
        public string VisitStatusAr { get; set; }
        public string Message { get; set; }

        [JsonIgnore] public bool IsSuccess { get; set; }
        [JsonIgnore] public int StatusCode { get; set; }
    }
}
