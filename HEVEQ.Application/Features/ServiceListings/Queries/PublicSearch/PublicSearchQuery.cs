using HEVEQ.Application.Features.ServiceListings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.Queries.PublicSearch
{
    public record PublicSearchQuery(
        string Query,
        string Context,
        string? Governorate,
        int Page = 1,
        int PageSize = 10) : IRequest<PublicSearchResultDto>;
}
