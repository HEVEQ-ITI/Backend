using HEVEQ.Application.Common.AI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Common.AI.Interfaces
{
    /// <summary>
    /// Validates a submitted review for profanity, contact-leak patterns, and semantic relevance to the
    /// listing/category it was posted against (e.g. a furniture-store review attached to a crane listing).
    /// Implemented in Infrastructure by ModeratorReviewContentValidator, backed by ReviewValidationPlugin.
    /// </summary>
    public interface IReviewContentValidator
    {
        Task<ReviewValidationResult> ValidateAsync(Guid reviewId, CancellationToken ct = default);
    }
}
