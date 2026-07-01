using HEVEQ.Application.Features.Documents.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.Commands.RejectDocument
{
    public record RejectDocumentCommand(Guid DocumentId, RejectDocumentRequest Request) : IRequest<RejectDocumentResponse>;
}

    public record RejectDocumentRequest(string? Reason);
