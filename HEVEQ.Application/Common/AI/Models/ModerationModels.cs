using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Common.AI.Models
{

    #region Risk Analysis 

    public enum RiskLevel { Low, Medium, High }
    public enum ModerationRecommendation { Approve, ApproveWithConditions, Reject }

    public sealed record PriceAnomalyDetail(
        bool IsAnomalous,
        decimal ListingPrice,
        decimal CategoryMedianPrice,
        decimal PercentOfMedian,
        string? Reason);

    public sealed record ContactLeakDetail(
        bool ContainsPhoneNumber,
        bool ContainsEmail,
        bool ContainsExternalPlatformReference,
        IReadOnlyList<string> RedactedSnippets);

    public sealed record DuplicateListingDetail(
        bool IsLikelyDuplicate,
        IReadOnlyList<Guid> SimilarListingIds,
        double HighestSimilarityScore);

    /// <summary>Maps directly onto ServiceListings.AiRiskScore / AiRiskLevel / AiRiskFlags / AiRecommendation.</summary>
    public sealed record ListingRiskReport(
        Guid ServiceListingId,
        int RiskScore,
        RiskLevel RiskLevel,
        ModerationRecommendation Recommendation,
        PriceAnomalyDetail PriceAnomaly,
        ContactLeakDetail ContactLeak,
        DuplicateListingDetail Duplicate,
        bool SpamDetected,
        IReadOnlyList<string> Flags,
        bool ShouldAutoBlock,
        DateTime AnalyzedAtUtc);

    #endregion

    #region Review Filtering

    /// <summary>Mirrors Reviews.ModerationStatus: Clean | FlaggedWarning | Blocked.</summary>
    public enum ReviewModerationOutcome { Clean, FlaggedWarning, Blocked }

    public sealed record ReviewValidationResult(
        Guid ReviewId,
        ReviewModerationOutcome Outcome,
        bool ContainsProfanity,
        bool ContainsContactLeak,
        bool IsSemanticMismatch,
        string? MismatchExplanation,
        IReadOnlyList<string> Flags);

    #endregion

    #region Complaint Triage 

    /// <summary>Mirrors Tickets.Priority / AiEscalationPriority: 0=Normal 1=High 2=Urgent.</summary>
    public enum TicketPriorityClassification { Normal = 0, High = 1, Urgent = 2 }

    public sealed record ComplaintTriageResult(
        Guid TicketId,
        string Summary,
        TicketPriorityClassification Priority,
        string PriorityRationale,
        IReadOnlyList<string> SafetyOrFinancialTriggers);

    #endregion
}
