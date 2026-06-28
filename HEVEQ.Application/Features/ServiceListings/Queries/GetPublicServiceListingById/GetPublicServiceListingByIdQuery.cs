using HEVEQ.Application.Features.ServiceListings.DTOs;
using MediatR;
using System;

namespace HEVEQ.Application.Features.ServiceListings.Queries.GetPublicServiceListingById;

// Request parameters: Just the listing ID from the URL
public record GetPublicServiceListingByIdQuery(Guid Id) : IRequest<PublicServiceListingDetailDto?>;