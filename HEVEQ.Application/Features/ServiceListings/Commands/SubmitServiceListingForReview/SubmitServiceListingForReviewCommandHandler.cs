using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Extensions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.ServiceListings.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.Commands.SubmitServiceListingForReview
{
    public class SubmitServiceListingForReviewCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
        : IRequestHandler<SubmitServiceListingForReviewCommand, SubmitForReviewResultDto>
    {
        private static readonly ServiceListingStatus[] AllowedSourceStatuses =
            [ServiceListingStatus.Draft, ServiceListingStatus.Rejected];

        public async Task<SubmitForReviewResultDto> Handle(
            SubmitServiceListingForReviewCommand request, CancellationToken cancellationToken)
        {

            if (!currentUserService.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var providerProfileId = await context.ProviderProfiles
                .AsNoTracking()
                .Where(p => p.UserId == currentUserService.UserId.Value)
                .Select(p => p.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (providerProfileId == Guid.Empty)
                throw new ForbiddenAccessException("Only providers can submit service listings for review.");


            var listing = await context.ServiceListings
                .Include(l => l.Photos)
                .Include(l => l.ServiceListingOperators).ThenInclude(slo => slo.Operator)
                .Include(l => l.Availability)
                .SingleOrDefaultAsync(l => l.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(ServiceListing), request.Id);


            if (listing.ProviderProfileId != providerProfileId)
                throw new ForbiddenAccessException("This listing does not belong to the current provider.");


            if (!AllowedSourceStatuses.Contains(listing.Status))
                throw new BadRequestException($"A listing in status '{listing.Status}' cannot be submitted for review.");


            var providerProfileComplete = await context.ProviderProfiles
                .AsNoTracking()
                .Where(p => p.Id == providerProfileId)
                .Select(p => !string.IsNullOrWhiteSpace(p.CompanyName) && p.BaseLatitude != 0 && p.BaseLongitude != 0)
                .SingleAsync(cancellationToken);


            var photosCount = listing.Photos.Count;
            var activeOperatorsCount = listing.ServiceListingOperators.Count(slo => slo.Operator.IsActive);
            var availabilityCount = listing.Availability.Count;

            var missing = new List<string>();
            if (photosCount < 3) missing.Add("At least 3 photos");
            if (activeOperatorsCount < 1) missing.Add("At least 1 operator");
            if (availabilityCount < 1) missing.Add("At least 1 availability schedule");
            if (!providerProfileComplete) missing.Add("Complete provider profile fields");

            if (missing.Count > 0)
                throw new BadRequestException($"Listing is not ready for review: {string.Join(", ", missing)}");


            listing.Status = ServiceListingStatus.PendingReview;
            listing.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);


            return new SubmitForReviewResultDto(
                Id: listing.Id,
                Status: listing.Status.ToString(),
                StatusAr: listing.Status.ToArabicText(),
                Message: "Listing submitted for review successfully"
            );
        }
    }
}