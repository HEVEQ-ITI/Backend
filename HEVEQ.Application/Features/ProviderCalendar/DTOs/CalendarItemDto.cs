using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ProviderCalendar.DTOs
{
    public record CalendarItemDto(
        string Type,            // "Booking" | "BlackoutDate"
        Guid? BookingId,
        Guid? ListingId,
        string Title,
        string? OperatorName,
        DateTime? Start,
        DateTime? End,
        DateOnly? Date,
        string? Status,
        string? StatusAr
    );
}
