using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Extensions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Documents.DTOs;
using HEVEQ.Application.Features.ServiceListings.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HEVEQ.Application.Features.ServiceListings.Queries.GetManageServiceListing
{
    public class GetManageServiceListingQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
        : IRequestHandler<GetManageServiceListingQuery, ManageServiceListingDto>
    {
        public async Task<ManageServiceListingDto> Handle(GetManageServiceListingQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var providerProfileId = await context.ProviderProfiles
                .AsNoTracking()
                .Where(p => p.UserId == currentUserService.UserId.Value)
                .Select(p => p.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (providerProfileId == Guid.Empty)
                throw new ForbiddenAccessException("Only providers can manage service listings.");

            var listing = await context.ServiceListings
                    .Include(l => l.Photos)
                    .Include(l => l.ServiceListingOperators).ThenInclude(slo => slo.Operator)
                    .Include(l => l.Availability)
                    .Include(l => l.BlackoutDates)
                    .Include(l => l.Documents)
                    .SingleOrDefaultAsync(l => l.Id == request.Id, cancellationToken)
                    ?? throw new NotFoundException(nameof(ServiceListing), request.Id);

            if (listing.ProviderProfileId != providerProfileId)
                throw new ForbiddenAccessException("This listing does not belong to the current provider.");

            var photosDto = listing.Photos
                .OrderBy(p => p.DisplayOrder)
                .Select(p => new ServiceListingPhotoDto
                {
                    Id = p.Id,
                    ListingId = p.ListingId,
                    PhotoUrl = p.PhotoUrl,
                    DisplayOrder = p.DisplayOrder
                }).ToList();

            var operatorsDto = listing.ServiceListingOperators
                .Where(slo => slo.Operator != null)
                .Select(slo => new ServiceListingOperatorDto
                {
                    OperatorId = slo.Operator.Id,
                    FullName = slo.Operator.FullName,
                    YearsOfExperience = slo.Operator.YearsOfExperience,
                    Specialization = slo.Operator.Specialization,
                    LicenseType = slo.Operator.LicenseType
                }).ToList();

            var availabilityDto = listing.Availability
                .OrderBy(a => a.DayOfWeek)
                .Select(a => new ServiceListingAvailabilityDto(
                    a.Id,
                    a.DayOfWeek,
                    a.DayOfWeek == 0 ? "Sunday" :
                    a.DayOfWeek == 1 ? "Monday" :
                    a.DayOfWeek == 2 ? "Tuesday" :
                    a.DayOfWeek == 3 ? "Wednesday" :
                    a.DayOfWeek == 4 ? "Thursday" :
                    a.DayOfWeek == 5 ? "Friday" : "Saturday",
                    a.OpenTime,
                    a.CloseTime
                )).ToList();

            var blackoutDatesDto = listing.BlackoutDates
                .Select(b => new BlackoutDateDto(
                    b.Id,
                    b.ListingId,
                    b.OperatorId,
                    b.Date,
                    b.Reason
                )).ToList();

            var documentsDto = listing.Documents
                .Select(d => new DocumentDto
                {
                    Id = d.Id,
                    UserId = d.UserId,
                    ServiceListingId = d.ServiceListingId,
                    MarketplaceListingId = d.MarketplaceListingId,
                    OperatorId = d.OperatorId,
                    DocumentType = d.DocumentType, 
                    FileUrl = d.FileUrl,
                    Status = d.Status,             
                    ExpiryDate = d.ExpiryDate,
                    FailureReason = d.FailureReason,
                    UploadedAt = d.UploadedAt,
                    VerifiedAt = d.VerifiedAt
                }).ToList();

            var providerProfileComplete = await context.ProviderProfiles
                .AsNoTracking()
                .Where(p => p.Id == providerProfileId)
                .Select(p => !string.IsNullOrWhiteSpace(p.CompanyName) && p.BaseLatitude != 0 && p.BaseLongitude != 0)
                .SingleAsync(cancellationToken);

            bool canSubmitForReview = false;
            var missingRequirements = new List<string>();

      
            if (listing.Status == ServiceListingStatus.Draft || listing.Status == ServiceListingStatus.Rejected)
            {
                var activeOperatorsCount = listing.ServiceListingOperators.Count(slo => slo.Operator != null && slo.Operator.IsActive);

                if (photosDto.Count < 3) missingRequirements.Add("At least 3 photos");
                if (activeOperatorsCount < 1) missingRequirements.Add("At least 1 operator");
                if (availabilityDto.Count < 1) missingRequirements.Add("At least 1 availability schedule");
                if (!providerProfileComplete) missingRequirements.Add("Complete provider profile fields");

                canSubmitForReview = missingRequirements.Count == 0;
            }

            return new ManageServiceListingDto(
                Id: listing.Id,
                Title: listing.Title,
                Description: listing.Description ?? string.Empty,
                CategoryId: listing.CategoryId,
                Status: listing.Status.ToString(),
                StatusAr: listing.Status.ToArabicText(),
                AdminRejectionNote: listing.AdminRejectionNote,
                QualityScore: listing.QualityScore,
                AiRiskScore: listing.AiRiskScore,
                AiRiskLevel: listing.AiRiskLevel,
                AiRecommendation: listing.AiRecommendation,
                Photos: photosDto,
                Operators: operatorsDto,
                Availability: availabilityDto,
                BlackoutDates: blackoutDatesDto,
                Documents: documentsDto,
                CanSubmitForReview: canSubmitForReview,
                MissingRequirements: missingRequirements
            );
        }
    }
}