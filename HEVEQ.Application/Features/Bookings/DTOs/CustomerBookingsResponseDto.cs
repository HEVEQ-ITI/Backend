using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class CustomerBookingsResponseDto
    {
        public IReadOnlyList<CustomerBookingListItemDto> Items { get; set; } = new List<CustomerBookingListItemDto>();
        public int TotalCount { get; set; }
    }
}
