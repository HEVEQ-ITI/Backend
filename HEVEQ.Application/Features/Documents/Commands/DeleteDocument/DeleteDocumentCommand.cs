using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.Commands.DeleteDocument
{
    public record DeleteDocumentCommand(Guid DocumentId) : IRequest;
    
}
