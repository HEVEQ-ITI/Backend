using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Auth.DTOs
{
    public class GetMeResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public bool ProfileCompleted { get; set; }
        public decimal? TrustScore { get; set; }
        public string DashboardUrl { get; set; }
    }
}
