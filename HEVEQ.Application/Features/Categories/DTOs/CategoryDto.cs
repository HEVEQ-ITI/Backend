using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Categories.DTOs
{
    public record CategoryDto(
        int Id,
        string Name,
        string Slug,
        int Type,
        int? ParentId
    );
}
