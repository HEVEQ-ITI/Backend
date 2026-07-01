namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class DisputeBookingResponseDto
    {
        public Guid BookingId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string StatusAr { get; set; } = string.Empty;
        public Guid? TicketId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}