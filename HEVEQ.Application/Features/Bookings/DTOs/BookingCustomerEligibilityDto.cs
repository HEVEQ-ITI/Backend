using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class BookingCustomerEligibilityDto
    {
        public bool CanBook { get; set; }
        public IReadOnlyList<string> MissingRequirements { get; set; } = new List<string>();
    }
}
