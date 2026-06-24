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

public record UpdateServiceListingRequest(
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

public record UpdateServiceListingCommand(Guid Id, UpdateServiceListingRequest Request) : IRequest;

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
            throw new ForbiddenAccessException("Only registered providers can manage service listings.");

        var listing = await context.ServiceListings
            .SingleOrDefaultAsync(l => l.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(ServiceListing), request.Id);

        if (listing.ProviderProfileId != providerProfileId)
            throw new ForbiddenAccessException("This listing does not belong to the current provider.");

        var categoryExists = await context.Categories
            .AsNoTracking()
            .AnyAsync(c => c.Id == request.Request.CategoryId, cancellationToken);

        if (!categoryExists)
            throw new NotFoundException(nameof(Category), request.Request.CategoryId);

        if (request.Request.HourlyRate.HasValue && request.Request.HourlyRate < 0)
            throw new ValidationException("HourlyRate", "HourlyRate must be zero or greater.");

        if (request.Request.DailyRate.HasValue && request.Request.DailyRate < 0)
            throw new ValidationException("DailyRate", "DailyRate must be zero or greater.");

        listing.CategoryId = request.Request.CategoryId;
        listing.Title = request.Request.Title;
        listing.Description = request.Request.Description;
        listing.Tags = request.Request.Tags;
        listing.EquipmentModel = request.Request.EquipmentModel;
        listing.EquipmentCapacity = request.Request.EquipmentCapacity;
        listing.EquipmentCondition = request.Request.EquipmentCondition;
        listing.YearOfManufacture = request.Request.YearOfManufacture;
        listing.EquipmentRegistrationNumber = request.Request.EquipmentRegistrationNumber;
        listing.HourlyRate = request.Request.HourlyRate;
        listing.DailyRate = request.Request.DailyRate;
        listing.MinimumBookingHours = request.Request.MinimumBookingHours;
        listing.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}