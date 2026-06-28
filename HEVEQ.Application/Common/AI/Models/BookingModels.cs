using ShareGear.Application.Common.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Common.AI.Models
{
  
        #region Pre-Booking Orchestration 

        public enum PreBookingDecision { Proceed, ProceedWithWarning, Block }

        public sealed record AgentTurnResult(
            string AgentName,
            string Verdict,
            string RawTurnText);

        public sealed record PreBookingDecisionResult(
            Guid BookingId,
            PreBookingDecision Decision,
            IReadOnlyList<AgentTurnResult> Turns,
            string FinalRationale);

        #endregion

        #region Re-Engagement 

        public sealed record ReengagementNotificationResult(
            Guid RejectedBookingId,
            IReadOnlyList<ServiceListingSnapshot> SuggestedAlternatives,
            string NotificationMessage);

        #endregion
    
}
