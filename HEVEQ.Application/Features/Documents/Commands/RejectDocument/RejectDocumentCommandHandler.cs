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

namespace HEVEQ.Application.Features.Documents.Commands.RejectDocument
{
    public class RejectDocumentCommandHandler(IApplicationDbContext context) : IRequestHandler<RejectDocumentCommand, RejectDocumentResponse>
    {
        public async Task<RejectDocumentResponse> Handle(RejectDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = await context.Documents
                .FirstOrDefaultAsync(d => d.Id == request.DocumentId, cancellationToken)
                ?? throw new NotFoundException(nameof(Document), request.DocumentId);

            if (document.Status != DocumentVerificationStatus.Pending)
                throw new ValidationException("Status",
                    $"Only pending documents can be rejected. Current status is '{document.Status}'.");

            document.Status = DocumentVerificationStatus.Rejected;
            document.FailureReason = request.Request.Reason;
            document.VerifiedAt = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);

            return new RejectDocumentResponse(
                document.Id,
                document.Status.ToString(),
                document.Status.ToArabic(),
                document.FailureReason);
        }
    }
}
