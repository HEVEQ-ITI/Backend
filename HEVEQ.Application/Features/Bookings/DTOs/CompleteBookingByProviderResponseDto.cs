using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class CompleteBookingByProviderResponseDto
    {
        public Guid BookingId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string StatusAr { get; set; } = string.Empty;
        public Guid EvidenceFormId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
