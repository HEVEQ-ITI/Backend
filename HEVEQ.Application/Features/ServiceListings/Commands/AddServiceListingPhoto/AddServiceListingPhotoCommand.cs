using MediatR;
using System;

namespace HEVEQ.Application.Features.ServiceListings.Commands.AddServiceListingPhoto;

public record AddServiceListingPhotoCommand(
    Guid ListingId,
    string PhotoUrl,
    int DisplayOrder
) : IRequest<Guid>;