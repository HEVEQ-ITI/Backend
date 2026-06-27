using HEVEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.DTOs
{
    public class DocumentDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? ServiceListingId { get; set; }
        public Guid? MarketplaceListingId { get; set; }
        public Guid? OperatorId { get; set; }
        public string DocumentType { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string StatusAr { get; set; } = string.Empty;
        public DateOnly? ExpiryDate { get; set; }
        public string? FailureReason { get; set; }
        public DateTime UploadedAt { get; set; }
        public DateTime? VerifiedAt { get; set; }
    }
}
