namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class ProviderBookingListItemDto
    {
        public Guid BookingId { get; set; }
        public string BookingNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string ServiceListingTitle { get; set; } = string.Empty;
        public DateOnly RequestedStartDate { get; set; }
        public TimeOnly RequestedStartTime { get; set; }
        public decimal EstimatedDurationHours { get; set; }
        public decimal EstimatedTotal { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool CanAccept { get; set; }
        public bool CanReject { get; set; }
    }
}