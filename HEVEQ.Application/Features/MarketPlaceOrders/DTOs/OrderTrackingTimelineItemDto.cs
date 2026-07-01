using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.DTOs
{
    public record OrderTrackingTimelineItemDto(string Label, string LabelAr, DateTime? Date, bool Done);
}
