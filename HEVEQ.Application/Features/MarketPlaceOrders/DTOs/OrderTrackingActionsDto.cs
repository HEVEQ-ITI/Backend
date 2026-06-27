using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.DTOs
{
    public class OrderTrackingActionsDto
    {
        public bool CanSellerConfirm { get; set; }
        public bool CanDispatch { get; set; }
        public bool CanMarkDelivered { get; set; }
        public bool CanComplete { get; set; }
        public bool CanCancel { get; set; }
    }
}
