using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class BookingCreateContextDto
    {
        public Guid ServiceListingId { get; set; }
        public string ServiceTitle { get; set; } = string.Empty;
        public string ProviderCompany { get; set; } = string.Empty;
        public decimal? HourlyRate { get; set; }
        public decimal? DailyRate { get; set; }
        public int MinimumBookingHours { get; set; }
        public IReadOnlyList<BookingCreateContextAvailabilityDto> Availability { get; set; }
            = new List<BookingCreateContextAvailabilityDto>();
        public BookingCreateContextAddressDto? DefaultAddress { get; set; }
        public BookingCustomerEligibilityDto CustomerEligibility { get; set; } = new();
    }
}
