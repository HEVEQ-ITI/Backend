using AutoMapper;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Documents.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.Queries.GetPendingDocuments
{
    public class GetPendingDocumentsQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetPendingDocumentsQuery, List<DocumentDto>>
    {
        public async Task<List<DocumentDto>> Handle(GetPendingDocumentsQuery request, CancellationToken cancellationToken)
        {
            var documents = await context.Documents
            .AsNoTracking()
            .Where(d => d.Status == DocumentVerificationStatus.Pending)
            .OrderBy(d => d.UploadedAt) // oldest first so admin reviews in order
            .ToListAsync(cancellationToken);

            return mapper.Map<List<DocumentDto>>(documents);
        }
    }
}
