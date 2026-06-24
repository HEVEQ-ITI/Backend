using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HEVEQ.Application.Features.ServiceListings.Commands;

public record LinkOperatorRequest(Guid OperatorId);

public record LinkOperatorToListingCommand(
    Guid ListingId,
    LinkOperatorRequest Request) : IRequest;

public class LinkOperatorToListingCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<LinkOperatorToListingCommand>
{
    public async Task Handle(LinkOperatorToListingCommand request, CancellationToken cancellationToken)
    {
        if (!currentUserService.UserId.HasValue)
            throw new ForbiddenAccessException("User is not authenticated.");

        var userId = currentUserService.UserId.Value;

        var providerProfileId = await context.ProviderProfiles
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .Select(p => p.Id)
            .SingleOrDefaultAsync(cancellationToken);

        var listing = await context.ServiceListings
            .AsNoTracking()
            .SingleOrDefaultAsync(l => l.Id == request.ListingId, cancellationToken)
            ?? throw new NotFoundException(nameof(ServiceListing), request.ListingId);

        if (listing.ProviderProfileId != providerProfileId)
            throw new ForbiddenAccessException("This listing does not belong to the current provider.");

        var operatorEntity = await context.Operators
            .AsNoTracking()
            .SingleOrDefaultAsync(o => o.Id == request.Request.OperatorId, cancellationToken)
            ?? throw new NotFoundException(nameof(Operator), request.Request.OperatorId);

        if (operatorEntity.ProviderProfileId != providerProfileId)
            throw new ForbiddenAccessException("This operator does not belong to the current provider.");

        if (operatorEntity.LicenseExpiryDate.HasValue && operatorEntity.LicenseExpiryDate.Value < DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ValidationException();

        var isOperatorBusy = await context.ServiceListingOperators
            .AsNoTracking()
            .AnyAsync(slo => slo.OperatorId == request.Request.OperatorId, cancellationToken);

        var alreadyLinked = await context.ServiceListingOperators
            .AsNoTracking()
            .AnyAsync(slo => slo.ListingId == request.ListingId && slo.OperatorId == request.Request.OperatorId, cancellationToken);

        if (alreadyLinked || isOperatorBusy)
            throw new ValidationException();

        context.ServiceListingOperators.Add(new ServiceListingOperator
        {
            ListingId = request.ListingId,
            OperatorId = request.Request.OperatorId
        });

        await context.SaveChangesAsync(cancellationToken);
    }
}