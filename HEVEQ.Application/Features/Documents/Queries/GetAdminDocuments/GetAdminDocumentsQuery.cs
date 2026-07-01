using HEVEQ.Application.Common.Models;
using HEVEQ.Application.Features.Documents.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.Queries.GetAdminDocuments
{
    public record GetAdminDocumentsQuery:IRequest<AdminPagedResult<AdminDocumentDto>>
    {
        public DocumentVerificationStatus? Status { get; init; }
        public DocumentType? DocumentType { get; init; }
        public string? Role { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 20;
    }
}
