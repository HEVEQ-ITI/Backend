using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ProviderCalendar.DTOs
{
    public record ProviderCalendarResultDto(List<CalendarItemDto> Items);
}
