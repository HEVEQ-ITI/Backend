namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class ProviderActiveJobItemDto
    {
        public Guid Id { get; set; }
        public string BookingNumber { get; set; } = string.Empty;
        public string ServiceTitle { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string OperatorName { get; set; } = string.Empty;
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public string Status { get; set; } = string.Empty;
        public string StatusAr { get; set; } = string.Empty;
        public bool CanStart { get; set; }
        public bool CanComplete { get; set; }
    }
}