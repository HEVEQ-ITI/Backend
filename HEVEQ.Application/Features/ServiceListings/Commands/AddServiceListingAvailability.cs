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

public record AddAvailabilityRequest(
    int DayOfWeek,
    TimeOnly OpenTime,
    TimeOnly CloseTime);

public record AddServiceListingAvailabilityCommand(
    Guid ListingId,
    AddAvailabilityRequest Request) : IRequest<Guid>;

public class AddServiceListingAvailabilityCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<AddServiceListingAvailabilityCommand, Guid>
{
    public async Task<Guid> Handle(
        AddServiceListingAvailabilityCommand request, CancellationToken cancellationToken)
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

        if (request.Request.DayOfWeek is < 0 or > 6)
            throw new ValidationException();

        if (request.Request.OpenTime >= request.Request.CloseTime ||
            request.Request.CloseTime.ToTimeSpan() - request.Request.OpenTime.ToTimeSpan() < TimeSpan.FromHours(2))
            throw new ValidationException();

        var hasBlackoutConflict = await context.BlackoutDates
            .AsNoTracking()
            .AnyAsync(b => b.ListingId == request.ListingId, cancellationToken);

        var duplicateExists = await context.ServiceListingAvailability
            .AsNoTracking()
            .AnyAsync(a => a.ListingId == request.ListingId && a.DayOfWeek == request.Request.DayOfWeek, cancellationToken);

        if (duplicateExists || hasBlackoutConflict)
            throw new ValidationException();

        var availability = new ServiceListingAvailability
        {
            Id = Guid.NewGuid(),
            ListingId = request.ListingId,
            DayOfWeek = request.Request.DayOfWeek,
            OpenTime = request.Request.OpenTime,
            CloseTime = request.Request.CloseTime
        };

        context.ServiceListingAvailability.Add(availability);
        await context.SaveChangesAsync(cancellationToken);

        return availability.Id;
    }
}