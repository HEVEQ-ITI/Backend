namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class CancelBookingResponseDto
    {
        public Guid BookingId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string StatusAr { get; set; } = string.Empty;
        public decimal RefundPercentage { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}