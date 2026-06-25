using ShareGear.Application.Common.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Common.AI.Interfaces
{
    public interface ISearchIntentExtractor
    {

        Task<SearchIntent> ExtractIntentAsync(
           string rawQuery,
           IReadOnlyList<ConversationTurn>? history = null,
           CancellationToken ct = default);
    }
}
