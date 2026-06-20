using HEVEQ.Domain.Enums;
using HEVEQ.Domain.Identity;

namespace HEVEQ.Domain.Entities;

public class Document
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid? UserId { get; set; }

    public Guid? ServiceListingId { get; set; }

    public Guid? MarketplaceListingId { get; set; }

    public Guid? OperatorId { get; set; }

    public DocumentType DocumentType { get; set; }

    public string FileUrl { get; set; } = string.Empty;

    public DocumentVerificationStatus Status { get; set; }

    public string? ExtractedText { get; set; }

    public decimal? ConfidenceScore { get; set; }

    public bool? KeyFieldsPresent { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public DocumentExpiryStatus? ExpiryStatus { get; set; }

    public string? FailureReason { get; set; }

    public DateTime UploadedAt { get; set; }

    public DateTime? VerifiedAt { get; set; }
    public ApplicationUser? User { get; set; }

    public ServiceListing? ServiceListing { get; set; }

    public MarketplaceListing? MarketplaceListing { get; set; }

    public Operator? Operator { get; set; }

}
