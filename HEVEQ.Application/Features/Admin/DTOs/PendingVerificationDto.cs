using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.DTOs
{
    public class PendingVerificationDto
    {
        public Guid DocumentId { get; set; }
        public string DocumentType { get; set; }
        public string FileUrl { get; set; }
        public string Status { get; set; }
        public DateTime UploadedAt { get; set; }
        public VerificationUserDto User { get; set; }
    }
}
