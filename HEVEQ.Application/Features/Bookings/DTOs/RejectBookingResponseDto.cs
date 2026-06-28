using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class RejectBookingResponseDto
    {
        public Guid Id { get; set; }
        public string BookingNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string StatusAr { get; set; } = string.Empty;
        public string ProviderRejectionReason { get; set; } = string.Empty;
    }
}
