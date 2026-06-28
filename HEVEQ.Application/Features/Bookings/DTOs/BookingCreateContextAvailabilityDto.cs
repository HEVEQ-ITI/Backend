using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class BookingCreateContextAvailabilityDto
    {
        public Guid Id { get; set; }
        public int DayOfWeek { get; set; }
        public string DayName { get; set; } = string.Empty;
        public string DayNameAr { get; set; } = string.Empty;
        public TimeOnly OpenTime { get; set; }
        public TimeOnly CloseTime { get; set; }
    }
}
