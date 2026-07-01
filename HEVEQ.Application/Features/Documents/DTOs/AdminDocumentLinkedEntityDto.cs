using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.DTOs
{
    public class AdminDocumentLinkedEntityDto
    {
        public string Type { get; set; } = string.Empty;
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
