using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.DTOs
{
    public class PendingActionDto
    {
        public string Type { get; set; } 
        public string Title { get; set; }
        public Guid ReferenceId { get; set; }
        public string Priority { get; set; } 
        public DateTime CreatedAt { get; set; }
        public string ActionUrl { get; set; }
    }
}
