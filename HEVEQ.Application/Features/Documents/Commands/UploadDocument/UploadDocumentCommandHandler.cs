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

namespace HEVEQ.Application.Features.Documents.Commands.UploadDocument
{
    public class UploadDocumentCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUser) : IRequestHandler<UploadDocumentCommand, UploadDocumentResponse>
    {
        public async Task<UploadDocumentResponse> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var req = request.Request;
            if (req.ServiceListingId.HasValue)
            {
                var exists = await context.ServiceListings
                    .AnyAsync(s => s.Id == req.ServiceListingId.Value, cancellationToken);
                if (!exists)
                    throw new NotFoundException(nameof(ServiceListing), req.ServiceListingId.Value);
            }

            if (req.MarketplaceListingId.HasValue)
            {
                var exists = await context.MarketplaceListings
                    .AnyAsync(m => m.Id == req.MarketplaceListingId.Value, cancellationToken);
                if (!exists)
                    throw new NotFoundException(nameof(MarketplaceListing), req.MarketplaceListingId.Value);
            }

            if (req.OperatorId.HasValue)
            {
                var exists = await context.Operators
                    .AnyAsync(o => o.Id == req.OperatorId.Value, cancellationToken);
                if (!exists)
                    throw new NotFoundException(nameof(Operator), req.OperatorId.Value);
            }

            var document = new Document
            {
                UserId = currentUser.UserId.Value,
                DocumentType = req.DocumentType,
                FileUrl = req.FileUrl,
                ExpiryDate = req.ExpiryDate,
                ServiceListingId = req.ServiceListingId,
                MarketplaceListingId = req.MarketplaceListingId,
                OperatorId = req.OperatorId,
                Status = DocumentVerificationStatus.Pending,
                UploadedAt = DateTime.UtcNow
            };

            context.Documents.Add(document);
            await context.SaveChangesAsync(cancellationToken);

            return new UploadDocumentResponse(
                document.Id,
                document.DocumentType.ToString(),
                document.FileUrl,
                document.Status.ToString(),
                document.Status.ToArabic(),
                document.UploadedAt
            );

        }
    }
}
