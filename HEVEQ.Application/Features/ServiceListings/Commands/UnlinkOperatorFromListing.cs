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

public record UnlinkOperatorFromListingCommand(Guid ListingId, Guid OperatorId) : IRequest;

public class UnlinkOperatorFromListingCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<UnlinkOperatorFromListingCommand>
{
    public async Task Handle(UnlinkOperatorFromListingCommand request, CancellationToken cancellationToken)
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

        var link = await context.ServiceListingOperators
            .SingleOrDefaultAsync(
                slo => slo.ListingId == request.ListingId && slo.OperatorId == request.OperatorId,
                cancellationToken)
            ?? throw new NotFoundException(nameof(ServiceListingOperator), request.OperatorId);

        context.ServiceListingOperators.Remove(link);
        await context.SaveChangesAsync(cancellationToken);
    }
}