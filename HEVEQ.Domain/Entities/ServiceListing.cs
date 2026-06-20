using HEVEQ.Domain.Enums;
using HEVEQ.Domain.Identity;

namespace HEVEQ.Domain.Entities;

public class ServiceListing
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ProviderProfileId { get; set; }

    public int CategoryId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string? Tags { get; set; }

    public string? EquipmentModel { get; set; }

    public string? EquipmentCapacity { get; set; }

    public EquipmentCondition? EquipmentCondition { get; set; }

    public int? YearOfManufacture { get; set; }

    public string? EquipmentRegistrationNumber { get; set; }

    public decimal? HourlyRate { get; set; }

    public decimal? DailyRate { get; set; }

    public int MinimumBookingHours { get; set; }

    public ServiceListingStatus Status { get; set; }

    public int? AiRiskScore { get; set; }

    public string? AiRiskLevel { get; set; }

    public string? AiRiskFlags { get; set; }

    public string? AiRecommendation { get; set; }

    public int? QualityScore { get; set; }

    public string? QdrantPointId { get; set; }

    public DateTime? LastEmbeddedAt { get; set; }

    public EmbeddingStatus EmbeddingStatus { get; set; }

    public string? AdminRejectionNote { get; set; }

    public Guid? RejectedByAdminId { get; set; }

    public DateTime? RejectedAt { get; set; }

    public int SubmissionCount { get; set; }

    public int? QsDescription { get; set; }

    public int? QsPhotos { get; set; }

    public int? QsSpecs { get; set; }

    public int? QsPricing { get; set; }

    public int? QsOperator { get; set; }

    public int? QsDocs { get; set; }

    public int? QsZone { get; set; }

    public DateTime? QualityScoreComputedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    public ProviderProfile ProviderProfile { get; set; } = null!;

    public Category Category { get; set; } = null!;

    public ApplicationUser? RejectedByAdmin { get; set; }

    public ICollection<ServiceListingPhoto> Photos { get; set; } = new List<ServiceListingPhoto>();

    public ICollection<ServiceListingOperator> ServiceListingOperators { get; set; } = new List<ServiceListingOperator>();

    public ICollection<ServiceListingAvailability> Availability { get; set; } = new List<ServiceListingAvailability>();

    public ICollection<BlackoutDate> BlackoutDates { get; set; } = new List<BlackoutDate>();

    public ICollection<Document> Documents { get; set; } = new List<Document>();

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

}
