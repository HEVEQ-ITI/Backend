using HEVEQ.Domain.Enums;
using HEVEQ.Domain.Identity;

namespace HEVEQ.Domain.Entities;

public class FieldVerificationForm
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid BookingId { get; set; }

    public Guid DispatchedEmployeeId { get; set; }

    public Guid DispatchedByAdminId { get; set; }

    public Guid? DecidedByAdminId { get; set; }

    public Guid LinkedEvidenceFormId { get; set; }

    public string? DispatchInstructions { get; set; }

    public VisitStatus VisitStatus { get; set; }

    public string? EmployeeNotes { get; set; }

    public FieldVerificationOutcome? FieldVerificationOutcome { get; set; }

    public decimal? AiSimilarityScore { get; set; }

    public string? AiSimilarityNotes { get; set; }

    public FieldVerificationAdminDecision AdminDecision { get; set; }

    public string? AdminDecisionNote { get; set; }

    public DateTime DispatchedAt { get; set; }

    public DateTime? VisitedAt { get; set; }

    public DateTime? FormSubmittedAt { get; set; }

    public DateTime? DecidedAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public Booking Booking { get; set; } = null!;

    public ApplicationUser DispatchedEmployee { get; set; } = null!;

    public ApplicationUser DispatchedByAdmin { get; set; } = null!;

    public ApplicationUser? DecidedByAdmin { get; set; }

    public JobCompletionEvidenceForm LinkedEvidenceForm { get; set; } = null!;

    public ICollection<FieldVerificationPhoto> Photos { get; set; } = new List<FieldVerificationPhoto>();

}
