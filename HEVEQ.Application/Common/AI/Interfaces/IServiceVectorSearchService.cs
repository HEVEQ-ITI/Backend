using ShareGear.Application.Common.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Common.AI.Interfaces
{
    /// <summary>
    /// Embeds the search intent, runs Qdrant similarity search over ServiceListings vectors, and produces
    /// a one-sentence RAG match explanation per hit for the Angular UI.
    /// Implemented in Infrastructure by QdrantServiceVectorSearchService, backed by SearchServicesPlugin.
    /// </summary>
    public interface IServiceVectorSearchService
    {
        Task<IReadOnlyList<VectorSearchHit>> SearchAsync(SearchIntent intent, int topK = 10, CancellationToken ct = default);

        Task<IReadOnlyDictionary<Guid, string>> ExplainMatchesBatchAsync(
            SearchIntent intent,
            IReadOnlyList<ServiceListingSnapshot> listings,
            CancellationToken ct = default);
    }

}
