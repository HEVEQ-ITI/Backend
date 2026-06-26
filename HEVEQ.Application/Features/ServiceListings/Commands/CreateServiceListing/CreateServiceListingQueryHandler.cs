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

namespace HEVEQ.Application.Features.ServiceListings.Commands.CreateServiceListing
{
    public class CreateServiceListingCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
        : IRequestHandler<CreateServiceListingCommand, CreateServiceListingResultDto>
    {
        public async Task<CreateServiceListingResultDto> Handle(
            CreateServiceListingCommand request, CancellationToken cancellationToken)
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
                throw new ForbiddenAccessException("Only a registered provider can create a service listing.");

            // Doc requirement: CategoryId must exist AND be a Service-type category
            // (a provider should never be able to attach their listing to a
            // Marketplace category).

            var category = await context.Categories
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken)
                ?? throw new NotFoundException(nameof(Category), request.CategoryId);

            if (category.Type != (int)CategoryType.Service)
                throw new ValidationException("CategoryId must reference a Service-type category.");

            if (request.HourlyRate < 0)
                throw new ValidationException("HourlyRate must be zero or greater.");

            if (request.MinimumBookingHours < 1)
                throw new ValidationException("MinimumBookingHours must be at least 1.");

            var listing = new ServiceListing
            {
                Id = Guid.NewGuid(),
                ProviderProfileId = providerProfileId,
                CategoryId = request.CategoryId,
                Title = request.Title,
                Description = request.Description,
                Tags = request.Tags,
                EquipmentModel = request.EquipmentModel,
                EquipmentCapacity = request.EquipmentCapacity,
                EquipmentCondition = request.EquipmentCondition,
                YearOfManufacture = request.YearOfManufacture,
                EquipmentRegistrationNumber = request.EquipmentRegistrationNumber,
                HourlyRate = request.HourlyRate,
                DailyRate = request.DailyRate,
                MinimumBookingHours = request.MinimumBookingHours,
                Status = ServiceListingStatus.Draft,           
                EmbeddingStatus = EmbeddingStatus.Pending,
                SubmissionCount = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.ServiceListings.Add(listing);
            await context.SaveChangesAsync(cancellationToken);

            return new CreateServiceListingResultDto
            {
                Id = listing.Id,
                Status = listing.Status.ToString(),
                StatusAr = listing.Status.ToArabicText(),
                NextStep = "AddPhotos",
                Message = "Service listing created successfully"
            };
        }
    }
}
