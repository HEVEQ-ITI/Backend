using HEVEQ.Application.Features.ServiceListings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.Queries.GetManageServiceListing
{
    public record GetManageServiceListingQuery(Guid Id) : IRequest<ManageServiceListingDto>;
}
