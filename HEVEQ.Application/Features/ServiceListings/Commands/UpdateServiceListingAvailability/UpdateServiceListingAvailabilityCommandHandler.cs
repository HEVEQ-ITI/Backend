using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HEVEQ.Application.Features.ServiceListings.Commands.UpdateServiceListingAvailability;

public class UpdateServiceListingAvailabilityCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<UpdateServiceListingAvailabilityCommand>
{
    public async Task Handle(UpdateServiceListingAvailabilityCommand request, CancellationToken cancellationToken)
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
            throw new ForbiddenAccessException("Only registered providers can manage availability.");


        var listing = await context.ServiceListings
            .SingleOrDefaultAsync(l => l.Id == request.ListingId, cancellationToken)
            ?? throw new NotFoundException(nameof(ServiceListing), request.ListingId);

        if (listing.ProviderProfileId != providerProfileId)
            throw new ForbiddenAccessException("This listing does not belong to the current provider.");

        var availability = await context.ServiceListingAvailability
            .SingleOrDefaultAsync(a => a.Id == request.AvailabilityId && a.ListingId == request.ListingId, cancellationToken)
            ?? throw new NotFoundException(nameof(ServiceListingAvailability), request.AvailabilityId);

     
        var duplicateExists = await context.ServiceListingAvailability
            .AsNoTracking()
            .AnyAsync(a => a.ListingId == request.ListingId && a.DayOfWeek == request.DayOfWeek && a.Id != request.AvailabilityId, cancellationToken);

        if (duplicateExists)
            throw new ValidationException("Availability", $"A schedule for day {request.DayOfWeek} already exists for this listing.");

    
        availability.DayOfWeek = request.DayOfWeek;
        availability.OpenTime = request.OpenTime;
        availability.CloseTime = request.CloseTime;

      
        listing.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}