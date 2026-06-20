using HEVEQ.Domain.Enums;
using HEVEQ.Domain.Identity;

namespace HEVEQ.Domain.Entities;

public class JobCompletionEvidenceForm
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid BookingId { get; set; }

    public Guid SubmittedByUserId { get; set; }

    public Guid? ReviewedByAdminId { get; set; }

    public string? ProviderNotes { get; set; }

    public EvidenceFormStatus Status { get; set; }

    public string? AdminReviewNote { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public Booking Booking { get; set; } = null!;

    public ApplicationUser SubmittedByUser { get; set; } = null!;

    public ApplicationUser? ReviewedByAdmin { get; set; }

    public ICollection<JobCompletionEvidencePhoto> Photos { get; set; } = new List<JobCompletionEvidencePhoto>();

    public ICollection<FieldVerificationForm> FieldVerificationForms { get; set; } = new List<FieldVerificationForm>();

}
