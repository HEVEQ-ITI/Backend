using HEVEQ.Application.Common.AI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Common.AI.Interfaces
{
    /// <summary>
    /// Summarizes an unstructured, emotional ticket thread into a clean 3-5 sentence summary and
    /// multi-class-classifies its priority (URGENT | HIGH | NORMAL).
    /// Implemented in Infrastructure by ModeratorComplaintTriageService, backed by ComplaintTriagePlugin.
    /// </summary>
    public interface IComplaintTriageService
    {
        Task<ComplaintTriageResult> TriageAsync(Guid ticketId, CancellationToken ct = default);
    }

}
