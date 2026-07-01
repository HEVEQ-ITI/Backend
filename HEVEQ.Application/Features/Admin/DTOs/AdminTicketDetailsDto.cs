using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.DTOs
{
    public class AdminTicketDetailsDto
    {
        public Guid Id { get; set; }
        public string TicketNumber { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }
        public string StatusAr { get; set; }

        public TicketSubmitterDto SubmittedBy { get; set; }
        public List<TicketMessageDto> Messages { get; set; } = new();

        public LinkedBookingDto LinkedBooking { get; set; }
        public LinkedMarketplaceOrderDto LinkedMarketplaceOrder { get; set; }
    }
}
