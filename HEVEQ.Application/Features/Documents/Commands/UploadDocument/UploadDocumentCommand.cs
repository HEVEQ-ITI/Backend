using HEVEQ.Application.Features.Documents.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.Commands.UploadDocument
{
    public record UploadDocumentCommand(UploadDocumentRequest Request) : IRequest<UploadDocumentResponse>;
    
}
public record UploadDocumentRequest(
    DocumentType DocumentType,
    string FileUrl,
    DateOnly? ExpiryDate,
    Guid? ServiceListingId,
    Guid? MarketplaceListingId,
    Guid? OperatorId);
