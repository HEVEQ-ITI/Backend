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

public record AddBlackoutDateRequest(
    DateOnly Date,
    string? Reason,
    Guid? OperatorId);

public record AddBlackoutDateCommand(Guid ListingId, AddBlackoutDateRequest Request) : IRequest<Guid>;

public class AddBlackoutDateCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<AddBlackoutDateCommand, Guid>
{
    public async Task<Guid> Handle(AddBlackoutDateCommand request, CancellationToken cancellationToken)
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

        if (request.Request.OperatorId is not null)
        {
            var operatorBelongsToProvider = await context.Operators
                .AsNoTracking()
                .AnyAsync(o => o.Id == request.Request.OperatorId && o.ProviderProfileId == providerProfileId,
                    cancellationToken);

            if (!operatorBelongsToProvider)
                throw new ForbiddenAccessException("This operator does not belong to the current provider.");
        }

        if (request.Request.Date < DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ValidationException();

        var duplicateExists = await context.BlackoutDates
            .AsNoTracking()
            .AnyAsync(b => b.ListingId == request.ListingId && b.Date == request.Request.Date, cancellationToken);

        if (duplicateExists)
            throw new ValidationException();

        var blackoutDate = new BlackoutDate
        {
            Id = Guid.NewGuid(),
            ListingId = request.ListingId,
            OperatorId = request.Request.OperatorId,
            Date = request.Request.Date,
            Reason = request.Request.Reason,
            CreatedAt = DateTime.UtcNow
        };

        context.BlackoutDates.Add(blackoutDate);
        await context.SaveChangesAsync(cancellationToken);

        return blackoutDate.Id;
    }
}