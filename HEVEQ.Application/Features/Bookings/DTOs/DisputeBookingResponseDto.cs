namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class DisputeBookingResponseDto
    {
        public Guid Id { get; set; }
        public string BookingNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string StatusAr { get; set; } = string.Empty;
        public DateTime? DisputeOpenedAt { get; set; }
        public Guid? TicketId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}