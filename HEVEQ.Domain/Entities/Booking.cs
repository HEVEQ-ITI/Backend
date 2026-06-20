using HEVEQ.Domain.Enums;
using NetTopologySuite.Geometries;
using HEVEQ.Domain.Identity;

namespace HEVEQ.Domain.Entities;

public class Booking
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CustomerId { get; set; }

    public Guid ServiceListingId { get; set; }

    public Guid? AssignedOperatorId { get; set; }

    public string JobTitle { get; set; } = string.Empty;

    public string? JobDescription { get; set; }

    public string Governorate { get; set; } = string.Empty;

    public string District { get; set; } = string.Empty;

    public string? Street { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public Point? ServiceLocationGeo { get; set; }

    public string? SiteContactName { get; set; }

    public string? SiteContactPhone { get; set; }

    public string? AccessRequirements { get; set; }

    public string? SafetyNotes { get; set; }

    public DateOnly RequestedStartDate { get; set; }

    public TimeOnly RequestedStartTime { get; set; }

    public decimal EstimatedDurationHours { get; set; }

    public decimal? ActualDurationHours { get; set; }

    public decimal HourlyRateSnapshot { get; set; }

    public decimal EstimatedTotal { get; set; }

    public decimal? SurchargeAmount { get; set; }

    public bool IsOutOfZoneBooking { get; set; }

    public decimal? OutOfZoneDistanceKm { get; set; }

    public decimal? OutOfZoneSurchargeAmount { get; set; }

    public DateTime? OutOfZoneSurchargeAcceptedAt { get; set; }

    public decimal? PrioritySurchargeAmount { get; set; }

    public decimal? FuelSurchargeAmount { get; set; }

    public BookingStatus Status { get; set; }

    public string? ProviderRejectionReason { get; set; }

    public string? CancellationReason { get; set; }

    public DateTime? CancelledAt { get; set; }

    public BookingCancellationInitiator? CancellationInitiatedByRole { get; set; }

    public decimal? CancellationRefundPct { get; set; }

    public bool ProviderCancellationPenaltyApplied { get; set; }

    public Guid? ReassignedToBookingId { get; set; }

    public Guid? OriginalBookingId { get; set; }

    public DateTime? ReassignedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ConfirmedAt { get; set; }

    public DateTime? PaymentCapturedAt { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? CompletedMarkedAt { get; set; }

    public DateTime? CompletionConfirmedAt { get; set; }

    public DateTime? DisputeOpenedAt { get; set; }

    public DateTime? FieldVerificationDispatchedAt { get; set; }

    public byte[] Timestamp { get; set; } = Array.Empty<byte>();
    public ApplicationUser Customer { get; set; } = null!;

    public ServiceListing ServiceListing { get; set; } = null!;

    public Operator? AssignedOperator { get; set; }

    public Booking? ReassignedToBooking { get; set; }

    public Booking? OriginalBooking { get; set; }

    public ICollection<Booking> ReassignedFromBookings { get; set; } = new List<Booking>();

    public ICollection<BookingTimeAdjustmentRequest> TimeAdjustmentRequests { get; set; } = new List<BookingTimeAdjustmentRequest>();

    public ICollection<OperatorAssignment> OperatorAssignments { get; set; } = new List<OperatorAssignment>();

    public ICollection<EscrowRecord> EscrowRecords { get; set; } = new List<EscrowRecord>();

    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public ICollection<JobCompletionEvidenceForm> JobCompletionEvidenceForms { get; set; } = new List<JobCompletionEvidenceForm>();

    public ICollection<FieldVerificationForm> FieldVerificationForms { get; set; } = new List<FieldVerificationForm>();

    public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

    public ICollection<ProviderIncident> ProviderIncidents { get; set; } = new List<ProviderIncident>();

}
