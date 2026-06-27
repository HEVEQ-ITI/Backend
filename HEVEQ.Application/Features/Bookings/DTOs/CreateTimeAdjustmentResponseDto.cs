namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class CreateTimeAdjustmentResponseDto
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public string BookingNumber { get; set; } = string.Empty;
        public decimal RequestedAdditionalHours { get; set; }
        public decimal AdditionalCostAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string StatusAr { get; set; } = string.Empty;
        public string ProviderNote { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}