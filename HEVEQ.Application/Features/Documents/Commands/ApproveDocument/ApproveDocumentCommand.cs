using HEVEQ.Application.Features.Documents.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.Commands.ApproveDocument
{
    public record ApproveDocumentCommand(Guid DocumentId) : IRequest<ApproveDocumentResponse>;
    
}
