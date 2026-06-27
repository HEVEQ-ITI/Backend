using AutoMapper;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Documents.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.Queries.GetMyDocuments
{
    public class GetMyDocumentsQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUser) : IRequestHandler<GetMyDocumentsQuery, List<DocumentDto>>
    {
        public async Task<List<DocumentDto>> Handle(GetMyDocumentsQuery request, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var documents = await context.Documents
            .AsNoTracking()
            .Where(d => d.UserId == currentUser.UserId.Value)
            .OrderByDescending(d => d.UploadedAt)
            .ToListAsync(cancellationToken);

            return mapper.Map<List<DocumentDto>>(documents);
        }
    }
}
