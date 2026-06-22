using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Categories.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Categories.Queries
{
    public record GetCategoriesQuery() : IRequest<List<CategoryDto>>;



    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetCategoriesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Categories.AsNoTracking()
                .Select(c => new CategoryDto(
                    c.Id,
                    c.Name,
                    c.Slug,
                    (int)c.Type,
                    c.ParentId
                ))
                .ToListAsync(cancellationToken);
        }
    }
}