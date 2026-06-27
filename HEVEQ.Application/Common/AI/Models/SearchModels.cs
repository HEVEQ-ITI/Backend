using System;
using System.Collections.Generic;
using System.Text;

namespace ShareGear.Application.Common.AI;

/// <summary>
/// Search domain models supporting the Sticky Chat conversational search flow: either a fully
/// hydrated RAG result set, or a clarifying-question turn that keeps the customer in the loop until
/// Location (service-zone routing) and EquipmentType (category routing) are both known.
/// </summary>

#region Conversation & Intent

public enum ConversationLanguage { English, EgyptianArabic }

public enum MissingSearchParameter { EquipmentType, Location }

/// <summary>A single prior turn in the Sticky Chat thread. The Angular client resends the full thread on
/// every follow-up call since the API is stateless — this is what lets a one-word reply like "Cairo" be
/// resolved against whatever the bot already asked for.</summary>
public sealed record ConversationTurn(string Role, string Content); // Role: "user" | "assistant"

/// <summary>
/// Structured search intent. EquipmentType and Location are nullable by design — a customer's first
/// message frequently omits one or both, which is exactly the condition that should halt the search and
/// trigger a clarifying turn instead of a blind, likely-poor vector search.
/// </summary>
public sealed record SearchIntent(
    string? EquipmentType,
    string? Location,
    string TaskDescription,
    ConversationLanguage Language)
{
    /// <summary>Location and EquipmentType are the two parameters that gate whether a search may proceed
    /// (service-zone check and category routing respectively).</summary>
    public IReadOnlyList<MissingSearchParameter> GetMissingParameters()
    {
        var missing = new List<MissingSearchParameter>(2);
        if (string.IsNullOrWhiteSpace(EquipmentType)) missing.Add(MissingSearchParameter.EquipmentType);
        if (string.IsNullOrWhiteSpace(Location)) missing.Add(MissingSearchParameter.Location);
        return missing;
    }

    public bool IsActionable => GetMissingParameters().Count == 0;
}

#endregion

#region Vector Search & Hydration

public sealed record VectorSearchHit(
    Guid ServiceListingId,
    float SimilarityScore,
    string QdrantPointId);

/// <summary>Hydrated, SQL-Server-backed projection of an active ServiceListing + its ProviderProfile.</summary>
public sealed record ServiceListingSnapshot(
    Guid Id,
    Guid ProviderProfileId,
    string Title,
    string Description,
    int CategoryId,
    decimal HourlyRate,
    decimal? DailyRate,
    string ProviderCompanyName,
    decimal ProviderAverageRating,
    double? DistanceKm);

public sealed record SearchResultItem(
    ServiceListingSnapshot Listing,
    float SimilarityScore,
    string MatchExplanation);

#endregion

#region Clarification (Sticky Chat)

/// <summary>A clarifying question turn, formulated in the customer's own detected language.</summary>
public sealed record ClarificationPrompt(
    IReadOnlyList<MissingSearchParameter> MissingParameters,
    string Message,
    ConversationLanguage Language);

#endregion

#region Dual-State Search Result

public enum SearchResponseStatus { ResultsReady, ClarificationNeeded }

/// <summary>
/// Single serializable response contract for the Sticky Chat endpoint. Modeled as one concrete record
/// with a discriminant (<see cref="Status"/>) rather than a polymorphic hierarchy, so the Angular client
/// deserializes one stable shape and branches on a single enum — exactly the dual-state behaviour the
/// floating chat assistant needs turn-to-turn, with no special-case deserialization logic on the frontend.
/// </summary>
public sealed record SearchServicesResult(
    SearchResponseStatus Status,
    SearchIntent? Intent,
    IReadOnlyList<SearchResultItem>? Results,
    bool HasZeroResults,
    ClarificationPrompt? Clarification,
    long ProcessingMs)
{
    public static SearchServicesResult ReadyWithResults(SearchIntent intent, IReadOnlyList<SearchResultItem> results, long processingMs) =>
        new(SearchResponseStatus.ResultsReady, intent, results, results.Count == 0, null, processingMs);

    public static SearchServicesResult NeedsClarification(SearchIntent partialIntent, ClarificationPrompt clarification, long processingMs) =>
        new(SearchResponseStatus.ClarificationNeeded, partialIntent, null, false, clarification, processingMs);
}

#endregion