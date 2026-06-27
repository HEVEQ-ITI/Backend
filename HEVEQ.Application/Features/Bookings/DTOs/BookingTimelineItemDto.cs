using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class BookingTimelineItemDto
    {
        public string Key { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string LabelAr { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public bool Done { get; set; }
    }
}
