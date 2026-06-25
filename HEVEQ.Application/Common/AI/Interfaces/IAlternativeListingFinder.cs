using HEVEQ.Application.Common.AI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Common.AI.Interfaces
{
    /// <summary>
    /// Builds the post-rejection re-engagement payload: fetches up to N nearby alternative active listings
    /// for the rejected booking's category/location and synthesizes a customer-facing notification message.
    /// Implemented in Infrastructure by PostRejectionAlternativesService.
    /// </summary>
    public interface IAlternativeListingFinder
    {
        Task<ReengagementNotificationResult> BuildReengagementAsync(Guid rejectedBookingId, int maxResults = 3, CancellationToken ct = default);
    }

}
