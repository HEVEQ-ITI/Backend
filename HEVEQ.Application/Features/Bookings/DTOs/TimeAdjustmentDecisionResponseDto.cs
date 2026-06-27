namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class TimeAdjustmentDecisionResponseDto
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public string BookingNumber { get; set; } = string.Empty;
        public decimal RequestedAdditionalHours { get; set; }
        public decimal AdditionalCostAmount { get; set; }
        public decimal BookingEstimatedDurationHours { get; set; }
        public decimal BookingEstimatedTotal { get; set; }
        public string Status { get; set; } = string.Empty;
        public string StatusAr { get; set; } = string.Empty;
        public DateTime? CustomerAcknowledgedAt { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}