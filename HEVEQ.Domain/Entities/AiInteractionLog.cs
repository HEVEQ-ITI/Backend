using HEVEQ.Domain.Enums;
using HEVEQ.Domain.Identity;

namespace HEVEQ.Domain.Entities;

public class AiInteractionLog
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public AiAgentType AgentType { get; set; }

    public string InvocationContext { get; set; } = string.Empty;

    public string? EntityType { get; set; }

    public Guid? EntityId { get; set; }

    public string? AiRecommendation { get; set; }

    public int? AiRiskScore { get; set; }

    public string? AdminOverride { get; set; }

    public Guid? AdminOverrideById { get; set; }

    public DateTime? AdminOverrideAt { get; set; }

    public int? LatencyMs { get; set; }

    public int? InputTokens { get; set; }

    public int? OutputTokens { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; private set; }
    public ApplicationUser? AdminOverrideBy { get; set; }

}
