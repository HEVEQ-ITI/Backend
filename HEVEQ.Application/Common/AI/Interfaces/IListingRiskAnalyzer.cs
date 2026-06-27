using HEVEQ.Application.Common.AI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Common.AI.Interfaces
{
    /// <summary>
    /// Runs the ModeratorAgent's risk &amp; compliance analysis for a ServiceListing: price anomaly
    /// (CategoryPricingAggregates-based), spam/duplicate detection (Qdrant + full-text), and contact-leak detection.
    /// Implemented in Infrastructure by ModeratorListingRiskAnalyzer, backed by ListingAnalysisPlugin + ContentModerationPlugin.
    /// </summary>
    public interface IListingRiskAnalyzer
    {
        Task<ListingRiskReport> AnalyzeServiceListingAsync(Guid serviceListingId, CancellationToken ct = default);
    }

}
