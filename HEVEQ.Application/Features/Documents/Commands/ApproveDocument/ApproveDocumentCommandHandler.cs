using AutoMapper;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Common.Localization;
using HEVEQ.Application.Features.Documents.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.Commands.ApproveDocument
{
    public class ApproveDocumentCommandHandler(IApplicationDbContext context) : IRequestHandler<ApproveDocumentCommand, ApproveDocumentResponse>
    {
        public async Task<ApproveDocumentResponse> Handle(ApproveDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = await context.Documents
            .FirstOrDefaultAsync(d => d.Id == request.DocumentId, cancellationToken)
            ?? throw new NotFoundException(nameof(Document), request.DocumentId);

            if (document.Status != DocumentVerificationStatus.Pending)
                throw new ValidationException(
                         "Status",
                         $"Only pending documents can be approved. Current status is '{document.Status}'.");

            document.Status = DocumentVerificationStatus.Approved;
            document.VerifiedAt = DateTime.UtcNow;
            document.FailureReason = null;

            await context.SaveChangesAsync(cancellationToken);

            return new ApproveDocumentResponse(
                    document.Id,
                    document.Status.ToString(),
                    document.Status.ToArabic(),
                    "Document approved successfully");
        }
    }
}
