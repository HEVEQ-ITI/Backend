using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HEVEQ.Application.Features.ServiceListings.Commands.UpdateServiceListing;

public class UpdateServiceListingCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<UpdateServiceListingCommand>
{
    public async Task Handle(UpdateServiceListingCommand request, CancellationToken cancellationToken)
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
            throw new ForbiddenAccessException("Only a registered provider can update a service listing.");

        var listing = await context.ServiceListings
            .SingleOrDefaultAsync(l => l.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(ServiceListing), request.Id);

        if (listing.ProviderProfileId != providerProfileId)
            throw new ForbiddenAccessException("This listing does not belong to the current provider.");

        switch (listing.Status)
        {
            case ServiceListingStatus.Active:
                listing.Status = ServiceListingStatus.PendingReview;
                listing.EmbeddingStatus = EmbeddingStatus.Pending;
                break;
            case ServiceListingStatus.Rejected:
                listing.Status = ServiceListingStatus.PendingReview;
                listing.SubmissionCount += 1;
                listing.AdminRejectionNote = null;
                break;
        }

        listing.CategoryId = request.CategoryId;
        listing.Title = request.Title;
        listing.Description = request.Description;
        listing.Tags = request.Tags;
        listing.EquipmentModel = request.EquipmentModel;
        listing.EquipmentCapacity = request.EquipmentCapacity;
        listing.EquipmentCondition = request.EquipmentCondition;
        listing.YearOfManufacture = request.YearOfManufacture;
        listing.EquipmentRegistrationNumber = request.EquipmentRegistrationNumber;
        listing.HourlyRate = request.HourlyRate;
        listing.DailyRate = request.DailyRate;
        listing.MinimumBookingHours = request.MinimumBookingHours;
        listing.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}