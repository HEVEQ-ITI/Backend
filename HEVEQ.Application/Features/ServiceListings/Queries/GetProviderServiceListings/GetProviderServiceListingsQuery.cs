using HEVEQ.Application.Features.ProviderProfiles.DTOs;
using HEVEQ.Application.Features.ServiceListings.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.Queries.GetProviderServiceListings
{

    public record GetProviderServiceListingsQuery(ServiceListingStatus? Status = null)
        : IRequest<ProviderServiceListingsResultDto>;
}
