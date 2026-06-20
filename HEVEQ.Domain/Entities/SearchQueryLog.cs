using HEVEQ.Domain.Enums;
using HEVEQ.Domain.Identity;

namespace HEVEQ.Domain.Entities;

public class SearchQueryLog
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid? UserId { get; set; }

    public string? SessionId { get; set; }

    public string RawQuery { get; set; } = string.Empty;

    public string? ExtractedIntentJson { get; set; }

    public SearchContextDomain ContextDomain { get; set; }

    public SearchMode SearchMode { get; set; }

    public int ResultCount { get; set; }

    public bool HasLowConfidence { get; set; }

    public bool HasZeroResults { get; set; }

    public bool AlternativeSuggested { get; set; }

    public DateTime? AlternativeAcceptedAt { get; set; }

    public int? ProcessingMs { get; set; }

    public DateTime CreatedAt { get; set; }
    public ApplicationUser? User { get; set; }

}
