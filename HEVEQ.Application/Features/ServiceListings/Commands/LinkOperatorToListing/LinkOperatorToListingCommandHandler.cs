using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.ServiceListings.DTOs;
using HEVEQ.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HEVEQ.Application.Features.ServiceListings.Commands.LinkOperatorToListing;

public class LinkOperatorToListingCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<LinkOperatorToListingCommand, LinkOperatorResultDto>
{
    public async Task<LinkOperatorResultDto> Handle(LinkOperatorToListingCommand request, CancellationToken cancellationToken)
    {
        if (!currentUserService.UserId.HasValue)
            throw new ForbiddenAccessException("User is not authenticated.");

        var userId = currentUserService.UserId.Value;

        var providerProfileId = await context.ProviderProfiles
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .Select(p => p.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (providerProfileId == Guid.Empty)
            throw new ForbiddenAccessException("Only registered providers can manage operators.");

        var listing = await context.ServiceListings
            .SingleOrDefaultAsync(l => l.Id == request.ListingId, cancellationToken)
            ?? throw new NotFoundException(nameof(ServiceListing), request.ListingId);

        if (listing.ProviderProfileId != providerProfileId)
            throw new ForbiddenAccessException("This listing does not belong to the current provider.");

        var operatorEntity = await context.Operators
            .AsNoTracking()
            .SingleOrDefaultAsync(o => o.Id == request.OperatorId, cancellationToken)
            ?? throw new NotFoundException(nameof(Operator), request.OperatorId);

        if (operatorEntity.ProviderProfileId != providerProfileId)
            throw new ForbiddenAccessException("This operator does not belong to the current provider.");

        if (!operatorEntity.IsActive)
            throw new ValidationException("Operator", "This operator is inactive and cannot be linked to a listing.");

        if (operatorEntity.LicenseExpiryDate.HasValue && operatorEntity.LicenseExpiryDate.Value < DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ValidationException("Operator", "Cannot link an operator with an expired license.");

        var alreadyLinked = await context.ServiceListingOperators
            .AsNoTracking()
            .AnyAsync(slo => slo.ListingId == request.ListingId && slo.OperatorId == request.OperatorId, cancellationToken);

        if (alreadyLinked)
            throw new ValidationException("Operator", "This operator is already linked to this listing.");

        context.ServiceListingOperators.Add(new ServiceListingOperator
        {
            ListingId = request.ListingId,
            OperatorId = request.OperatorId
        });

        listing.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        return new LinkOperatorResultDto("Operator linked to listing successfully");
    }
}