using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Categories.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;       
        public int? ParentId { get; set; }                      
    }
}
