using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HEVEQ.Application.Features.Admin.DTOs
{
    public class FieldVerificationDecisionResponse
    {
        public Guid FieldVerificationFormId { get; set; }
        public string AdminDecision { get; set; }
        public string AdminDecisionAr { get; set; }
        public string Message { get; set; }

        [JsonIgnore] public bool IsSuccess { get; set; }
        [JsonIgnore] public int StatusCode { get; set; }
    }
}
