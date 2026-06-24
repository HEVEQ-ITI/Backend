namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public Guid ServiceListingId { get; set; }
        public string ServiceListingTitle { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string? JobDescription { get; set; }
        public string Governorate { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string? Street { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateOnly RequestedStartDate { get; set; }
        public TimeOnly RequestedStartTime { get; set; }
        public decimal EstimatedDurationHours { get; set; }
        public decimal HourlyRateSnapshot { get; set; }
        public decimal EstimatedTotal { get; set; }
        public decimal? SurchargeAmount { get; set; }
        public bool IsOutOfZoneBooking { get; set; }
        public decimal? OutOfZoneDistanceKm { get; set; }
        public decimal? OutOfZoneSurchargeAmount { get; set; }
        public DateTime? OutOfZoneSurchargeAcceptedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public Guid? AssignedOperatorId { get; set; }
        public string? ProviderRejectionReason { get; set; }
        public string? CancellationReason { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public DateTime? PaymentCapturedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedMarkedAt { get; set; }
        public DateTime? CompletionConfirmedAt { get; set; }
        public DateTime? DisputeOpenedAt { get; set; }
    }
}