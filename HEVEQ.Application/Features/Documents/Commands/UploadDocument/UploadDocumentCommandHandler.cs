using AutoMapper;
using HEVEQ.Application.Common.Interfaces;
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
    public class UploadDocumentCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<UploadDocumentCommand, DocumentDto>
    {
        public async Task<DocumentDto> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {
            var req = request.Request;
            if (req.ServiceListingId.HasValue)
            {
                var exists = await context.ServiceListings
                    .AnyAsync(s => s.Id == req.ServiceListingId.Value, cancellationToken);
                if (!exists)
                    throw new KeyNotFoundException($"Service listing with ID {req.ServiceListingId} was not found.");
            }

            if (req.MarketplaceListingId.HasValue)
            {
                var exists = await context.MarketplaceListings
                    .AnyAsync(m => m.Id == req.MarketplaceListingId.Value, cancellationToken);
                if (!exists)
                    throw new KeyNotFoundException($"Marketplace listing with ID {req.MarketplaceListingId} was not found.");
            }

            if (req.OperatorId.HasValue)
            {
                var exists = await context.Operators
                    .AnyAsync(o => o.Id == req.OperatorId.Value, cancellationToken);
                if (!exists)
                    throw new KeyNotFoundException($"Operator with ID {req.OperatorId} was not found.");
            }

            var document = new Document
            {
                UserId = request.UserId,
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

            return mapper.Map<DocumentDto>(document);

        }
    }
}
