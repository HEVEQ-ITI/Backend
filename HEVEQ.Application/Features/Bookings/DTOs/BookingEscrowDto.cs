namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class BookingEscrowDto
    {
        public Guid BookingId { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal PlatformCommission { get; set; }
        public decimal ProviderPayout { get; set; }
        public decimal VatAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string StatusAr { get; set; } = string.Empty;
        public DateTime? CapturedAt { get; set; }
        public DateTime? ReleasedAt { get; set; }
        public DateTime? FrozenAt { get; set; }
    }
}