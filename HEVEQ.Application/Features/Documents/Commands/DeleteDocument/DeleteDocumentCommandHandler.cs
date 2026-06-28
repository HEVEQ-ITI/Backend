using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.Commands.DeleteDocument
{
    public class DeleteDocumentCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser) : IRequestHandler<DeleteDocumentCommand>
    {
        public async Task Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var document = await context.Documents
                .FirstOrDefaultAsync(d => d.Id == request.DocumentId, cancellationToken)
                ?? throw new NotFoundException(nameof(Document), request.DocumentId);

            if (document.UserId != currentUser.UserId.Value)
                throw new ForbiddenAccessException("You are not authorized to delete this document.");

            if (document.Status == DocumentVerificationStatus.Approved)
                throw new ValidationException("Status", "An approved document cannot be deleted.");

            context.Documents.Remove(document);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
