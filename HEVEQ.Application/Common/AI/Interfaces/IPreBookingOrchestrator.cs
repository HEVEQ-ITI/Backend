using HEVEQ.Application.Common.AI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Common.AI.Interfaces
{
    /// <summary>
    /// Runs the strict two-turn AgentGroupChat pre-booking evaluation: ConciergeAgent (geo-fencing +
    /// timeline validation) followed by ModeratorAgent (provider trust, history, security flags).
    /// Implemented in Infrastructure by PreBookingOrchestrationService.
    /// </summary>
    public interface IPreBookingOrchestrator
    {
        Task<PreBookingDecisionResult> EvaluateAsync(Guid bookingId, CancellationToken ct = default);
    }
}
