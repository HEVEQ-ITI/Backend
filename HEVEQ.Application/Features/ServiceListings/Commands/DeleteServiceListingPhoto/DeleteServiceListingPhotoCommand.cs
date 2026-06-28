using MediatR;
using System;

namespace HEVEQ.Application.Features.ServiceListings.Commands.DeleteServiceListingPhoto;

public record DeleteServiceListingPhotoCommand(Guid ListingId, Guid PhotoId) : IRequest;