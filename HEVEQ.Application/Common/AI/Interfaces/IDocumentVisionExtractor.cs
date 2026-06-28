using HEVEQ.Application.Common.AI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Common.AI.Interfaces
{
    /// <summary>
    /// Extracts structured fields from an uploaded verification document (Documents table) using GPT-4o's
    /// native vision capability — no Azure AI Document Intelligence dependency, per the OpenAI-only constraint.
    /// Implemented in Infrastructure by GptVisionDocumentExtractor.
    /// </summary>
    public interface IDocumentVisionExtractor
    {
        Task<DocumentExtractionResult> ExtractAsync(Guid documentId, Stream fileStream, string mimeType, CancellationToken ct = default);
    }
}
