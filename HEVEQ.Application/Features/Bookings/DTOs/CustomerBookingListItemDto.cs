using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class CustomerBookingListItemDto
    {
        public Guid BookingId { get; set; }
        public string BookingNumber { get; set; } = string.Empty;
        public string ServiceListingTitle { get; set; } = string.Empty;
        public string ProviderName { get; set; } = string.Empty;
        public DateOnly RequestedStartDate { get; set; }
        public TimeOnly RequestedStartTime { get; set; }
        public decimal EstimatedDurationHours { get; set; }
        public decimal EstimatedTotal { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
