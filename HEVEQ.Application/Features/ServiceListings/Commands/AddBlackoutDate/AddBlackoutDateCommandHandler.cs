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

namespace HEVEQ.Application.Features.ServiceListings.Commands.AddBlackoutDate;

public class AddBlackoutDateCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<AddBlackoutDateCommand, BlackoutDateDto>
{
    public async Task<BlackoutDateDto> Handle(AddBlackoutDateCommand request, CancellationToken cancellationToken)
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
            throw new ForbiddenAccessException("Only registered providers can manage blackout dates.");

 
        var listing = await context.ServiceListings
            .SingleOrDefaultAsync(l => l.Id == request.ListingId, cancellationToken)
            ?? throw new NotFoundException(nameof(ServiceListing), request.ListingId);

        if (listing.ProviderProfileId != providerProfileId)
            throw new ForbiddenAccessException("This listing does not belong to the current provider.");


        if (request.OperatorId is not null)
        {
            var operatorBelongsToProvider = await context.Operators
                .AsNoTracking()
                .AnyAsync(o => o.Id == request.OperatorId && o.ProviderProfileId == providerProfileId, cancellationToken);

            if (!operatorBelongsToProvider)
                throw new ForbiddenAccessException("This operator does not belong to the current provider.");
        }


        var duplicateExists = await context.BlackoutDates
            .AsNoTracking()
            .AnyAsync(b => b.ListingId == request.ListingId
                        && b.Date == request.Date
                        && b.OperatorId == request.OperatorId, cancellationToken);

        if (duplicateExists)
            throw new ValidationException("BlackoutDate", "A blackout entry already exists for this date and operator.");

       
        var blackoutDate = new BlackoutDate
        {
            Id = Guid.NewGuid(),
            ListingId = request.ListingId,
            OperatorId = request.OperatorId,
            Date = request.Date,
            Reason = request.Reason,
            CreatedAt = DateTime.UtcNow
        };

        context.BlackoutDates.Add(blackoutDate);


        listing.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

   
        return new BlackoutDateDto(
            blackoutDate.Id,
            blackoutDate.ListingId,
            blackoutDate.OperatorId,
            blackoutDate.Date,
            blackoutDate.Reason);
    }
}