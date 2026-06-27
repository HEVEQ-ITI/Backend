using HEVEQ.Application.Features.Categories.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Categories.Queries.GetCategories
{
    public record GetCategoriesQuery(CategoryType? Type = null) : IRequest<List<CategoryDto>>;
}
