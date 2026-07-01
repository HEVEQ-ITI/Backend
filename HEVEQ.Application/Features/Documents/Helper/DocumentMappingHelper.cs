using HEVEQ.Application.Features.Documents.DTOs;
using HEVEQ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.Helper
{
    public static class DocumentMappingHelper
    {
        public static AdminDocumentLinkedEntityDto? ResolveLinkedEntity(Document document)
        {
            if (document.ServiceListing is not null)
            {
                return new AdminDocumentLinkedEntityDto
                {
                    Type = "ServiceListing",
                    Id = document.ServiceListing.Id,
                    Title = document.ServiceListing.Title
                };
            }

            if (document.MarketplaceListing is not null)
            {
                return new AdminDocumentLinkedEntityDto
                {
                    Type = "MarketplaceListing",
                    Id = document.MarketplaceListing.Id,
                    Title = document.MarketplaceListing.Title
                };
            }

            if (document.Operator is not null)
            {
                return new AdminDocumentLinkedEntityDto
                {
                    Type = "Operator",
                    Id = document.Operator.Id,
                    Title = document.Operator.FullName
                };
            }

            return null;
        }
    }
}
