using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HEVEQ.Application.Features.Admin.DTOs
{
    public class RejectServiceListingResponse
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public string StatusAr { get; set; }
        public string AdminRejectionNote { get; set; }
        [JsonIgnore] public bool IsSuccess { get; set; }
        [JsonIgnore] public int StatusCode { get; set; }
        [JsonIgnore] public string Message { get; set; }
    }
}
