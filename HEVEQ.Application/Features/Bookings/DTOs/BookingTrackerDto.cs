namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class BookingTrackerDto
    {
        public Guid BookingId { get; set; }
        public string BookingNumber { get; set; } = string.Empty;
        public string CurrentStatus { get; set; } = string.Empty;
        public string CurrentStatusAr { get; set; } = string.Empty;
        public IReadOnlyList<BookingTimelineItemDto> Timeline { get; set; } = new List<BookingTimelineItemDto>();
        public BookingNextActionDto NextAction { get; set; } = new();
        public BookingActionsDto AvailableActions { get; set; } = new();
    }
}