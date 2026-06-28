using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HEVEQ.Application.Features.Admin.DTOs
{
    public class UpdateUserStatusDTO
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public string Message { get; set; }

        [JsonIgnore] public bool IsSuccess { get; set; }
        [JsonIgnore] public int StatusCode { get; set; }
    }
}
