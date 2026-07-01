using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class BookingActionsDto
    {
        public bool CanAccept { get; set; }
        public bool CanReject { get; set; }
        public bool CanStart { get; set; }
        public bool CanComplete { get; set; }
        public bool CanConfirmCompletion { get; set; }
        public bool CanDispute { get; set; }
        public bool CanCancel { get; set; }
        public bool CanProviderCancel { get; set; }
        public bool CanPay { get; set; }
    }
}
