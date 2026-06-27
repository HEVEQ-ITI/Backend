using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.DTOs
{
    public record UploadDocumentResponse(
    Guid Id,
    string DocumentType,
    string FileUrl,
    string Status,
    string StatusAr,
    DateTime UploadedAt);
}
