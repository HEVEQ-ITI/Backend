using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.DTOs
{
    public record ApproveDocumentResponse(
    Guid Id,
    string Status,
    string StatusAr,
    string Message);
}
