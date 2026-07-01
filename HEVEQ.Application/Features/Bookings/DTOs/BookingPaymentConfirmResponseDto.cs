namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class BookingPaymentConfirmResponseDto
    {
        public Guid BookingId { get; set; }
        public string BookingStatus { get; set; } = string.Empty;
        public string BookingStatusAr { get; set; } = string.Empty;
        public string EscrowStatus { get; set; } = string.Empty;
        public string EscrowStatusAr { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}