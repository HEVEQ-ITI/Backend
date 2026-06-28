using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Categories.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Categories.Queries.GetCategories
{
    public class GetCategoriesQueryHandler(IApplicationDbContext context)
        : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
    {
        public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var query = context.Categories.AsNoTracking();

            if (request.Type is not null)
                query = query.Where(c => c.Type == request.Type);

            return await query
                .OrderBy(c => c.ParentId)  
                .ThenBy(c => c.Name)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Slug = c.Slug,
                    Type = ((CategoryType)c.Type).ToString(),
                    ParentId = c.ParentId
                })
                .ToListAsync(cancellationToken);
        }
    }
}
