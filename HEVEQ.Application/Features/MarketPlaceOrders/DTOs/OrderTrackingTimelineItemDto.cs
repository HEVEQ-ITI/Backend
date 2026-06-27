using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.DTOs
{
    public record OrderTrackingTimelineItemDto(string Label, DateTime? Date, bool Done);
}
