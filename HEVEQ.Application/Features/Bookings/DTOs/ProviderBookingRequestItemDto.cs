namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class ProviderBookingRequestItemDto
    {
        public Guid Id { get; set; }
        public string BookingNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string ServiceTitle { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateOnly RequestedStartDate { get; set; }
        public TimeOnly RequestedStartTime { get; set; }
        public decimal EstimatedDurationHours { get; set; }
        public decimal EstimatedTotal { get; set; }
        public string Status { get; set; } = string.Empty;
        public string StatusAr { get; set; } = string.Empty;
        public bool CanAccept { get; set; }
        public bool CanReject { get; set; }
    }
}