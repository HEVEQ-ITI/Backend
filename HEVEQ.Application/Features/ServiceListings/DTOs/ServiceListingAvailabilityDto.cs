using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.DTOs
{
    public record ServiceListingAvailabilityDto(
        Guid Id,
        int DayOfWeek,
        string DayName, 
        TimeOnly OpenTime,
        TimeOnly CloseTime);
}
