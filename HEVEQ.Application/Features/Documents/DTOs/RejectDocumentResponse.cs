using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.DTOs
{
    public record RejectDocumentResponse(
         Guid Id,
         string Status,
         string StatusAr,
         string? FailureReason);
}
