using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.DTOs
{
    public class ReviewPricingDto
    {
        public decimal? HourlyRate { get; set; }
        public decimal? DailyRate { get; set; }
        public int MinimumBookingHours { get; set; }
    }
}
