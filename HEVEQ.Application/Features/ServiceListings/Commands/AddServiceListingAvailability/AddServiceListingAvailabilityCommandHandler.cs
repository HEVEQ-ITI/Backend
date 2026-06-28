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

namespace HEVEQ.Application.Features.ServiceListings.Commands.AddServiceListingAvailability;

public class AddServiceListingAvailabilityCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<AddServiceListingAvailabilityCommand, ServiceListingAvailabilityDto> 
{
    public async Task<ServiceListingAvailabilityDto> Handle(
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

        if (providerProfileId == Guid.Empty)
            throw new ForbiddenAccessException("Only registered providers can manage availability.");

    
        var listing = await context.ServiceListings
            .SingleOrDefaultAsync(l => l.Id == request.ListingId, cancellationToken)
            ?? throw new NotFoundException(nameof(ServiceListing), request.ListingId);

        if (listing.ProviderProfileId != providerProfileId)
            throw new ForbiddenAccessException("This listing does not belong to the current provider.");

   
        var duplicateExists = await context.ServiceListingAvailability
            .AsNoTracking()
            .AnyAsync(a => a.ListingId == request.ListingId && a.DayOfWeek == request.DayOfWeek, cancellationToken);

        if (duplicateExists)
            throw new ValidationException("Availability", $"A schedule for day {request.DayOfWeek} already exists for this listing.");

    
        var availability = new ServiceListingAvailability
        {
            Id = Guid.NewGuid(),
            ListingId = request.ListingId,
            DayOfWeek = request.DayOfWeek,
            OpenTime = request.OpenTime,
            CloseTime = request.CloseTime
        };

        context.ServiceListingAvailability.Add(availability);

        listing.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

      
        var dayName = ((DayOfWeek)request.DayOfWeek).ToString();


        return new ServiceListingAvailabilityDto(
            availability.Id,
            availability.DayOfWeek,
            dayName,
            availability.OpenTime,
            availability.CloseTime);
    }
}