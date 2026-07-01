using HEVEQ.Application.Features.Admin.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Query.GetMarketplaceListingReviewDetails
{
    public class GetMarketplaceListingReviewDetailsQuery : IRequest<MarketplaceListingReviewDetailsDto>
    {
        public Guid Id { get; set; }
    }
}
