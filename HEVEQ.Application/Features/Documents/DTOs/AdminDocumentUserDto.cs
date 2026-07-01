using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.DTOs
{
    public  class AdminDocumentUserDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
