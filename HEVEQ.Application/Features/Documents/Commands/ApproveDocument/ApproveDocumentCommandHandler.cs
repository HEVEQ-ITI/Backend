using AutoMapper;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Documents.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.Commands.ApproveDocument
{
    public class ApproveDocumentCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<ApproveDocumentCommand, DocumentDto>
    {
        public async Task<DocumentDto> Handle(ApproveDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = await context.Documents
            .FirstOrDefaultAsync(d => d.Id == request.DocumentId, cancellationToken)
            ?? throw new KeyNotFoundException($"Document with ID {request.DocumentId} was not found.");

            if (document.Status != DocumentVerificationStatus.Pending)
                throw new InvalidOperationException(
                    $"Only pending documents can be approved. Current status is '{document.Status}'.");

            document.Status = DocumentVerificationStatus.Approved;
            document.VerifiedAt = DateTime.UtcNow;
            document.FailureReason = null;

            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<DocumentDto>(document);
        }
    }
}
