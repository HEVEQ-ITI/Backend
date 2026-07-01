using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.DTOs
{
    public class AdminDocumentDto
    {
        public Guid DocumentId { get; set; }
        public string DocumentType { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string StatusAr { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
        public DateOnly? ExpiryDate { get; set; }
        public string? ExpiryStatus { get; set; }
        public string? ExtractedText { get; set; }
        public decimal? ConfidenceScore { get; set; }
        public bool? KeyFieldsPresent { get; set; }
        public AdminDocumentUserDto? User { get; set; }
        public AdminDocumentLinkedEntityDto? LinkedEntity { get; set; }
    }
}
