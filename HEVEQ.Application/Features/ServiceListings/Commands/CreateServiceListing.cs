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

namespace HEVEQ.Application.Features.ServiceListings.Commands;

public record CreateServiceListingRequest(
    int CategoryId,
    string Title,
    string Description,
    string? Tags,
    string? EquipmentModel,
    string? EquipmentCapacity,
    EquipmentCondition? EquipmentCondition,
    int? YearOfManufacture,
    string? EquipmentRegistrationNumber,
    decimal? HourlyRate,
    decimal? DailyRate,
    int MinimumBookingHours);

public record CreateServiceListingCommand(CreateServiceListingRequest Request) : IRequest<Guid>;

public class CreateServiceListingCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<CreateServiceListingCommand, Guid>
{
    public async Task<Guid> Handle(CreateServiceListingCommand request, CancellationToken cancellationToken)
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

        var categoryExists = await context.Categories
            .AsNoTracking()
            .AnyAsync(c => c.Id == request.Request.CategoryId, cancellationToken);

        if (!categoryExists)
            throw new NotFoundException(nameof(Category), request.Request.CategoryId);

        if (request.Request.HourlyRate.HasValue && request.Request.HourlyRate < 0)
            throw new ValidationException("HourlyRate", "HourlyRate must be zero or greater.");

        if (request.Request.DailyRate.HasValue && request.Request.DailyRate < 0)
            throw new ValidationException("DailyRate", "DailyRate must be zero or greater.");

        var listing = new ServiceListing
        {
            Id = Guid.NewGuid(),
            ProviderProfileId = providerProfileId,
            CategoryId = request.Request.CategoryId,
            Title = request.Request.Title,
            Description = request.Request.Description,
            Tags = request.Request.Tags,
            EquipmentModel = request.Request.EquipmentModel,
            EquipmentCapacity = request.Request.EquipmentCapacity,
            EquipmentCondition = request.Request.EquipmentCondition,
            YearOfManufacture = request.Request.YearOfManufacture,
            EquipmentRegistrationNumber = request.Request.EquipmentRegistrationNumber,
            HourlyRate = request.Request.HourlyRate,
            DailyRate = request.Request.DailyRate,
            MinimumBookingHours = request.Request.MinimumBookingHours,
            Status = ServiceListingStatus.PendingReview,
            EmbeddingStatus = EmbeddingStatus.Pending,
            SubmissionCount = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        context.ServiceListings.Add(listing);
        await context.SaveChangesAsync(cancellationToken);

        return listing.Id;
    }
}