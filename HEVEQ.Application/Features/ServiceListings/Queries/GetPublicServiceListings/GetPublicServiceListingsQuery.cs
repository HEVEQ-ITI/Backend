using HEVEQ.Application.Features.ServiceListings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.Queries.GetPublicServiceListings
{
    public record GetPublicServiceListingsQuery(
       string? Search = null,
       int? CategoryId = null,
       string? Governorate = null,
       decimal? MinRate = null,
       decimal? MaxRate = null,
       int Page = 1,
       int PageSize = 10
   ) : IRequest<PublicPaginatedList<PublicServiceListingDto>>;
}
