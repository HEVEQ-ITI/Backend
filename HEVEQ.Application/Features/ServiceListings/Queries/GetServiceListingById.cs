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

namespace HEVEQ.Application.Features.ServiceListings.Queries;

public record GetServiceListingByIdQuery(Guid Id) : IRequest<ServiceListingDetailDto>;

public class GetServiceListingByIdQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetServiceListingByIdQuery, ServiceListingDetailDto>
{
    public async Task<ServiceListingDetailDto> Handle(
        GetServiceListingByIdQuery request, CancellationToken cancellationToken)
    {
        var listing = await context.ServiceListings
            .AsNoTracking()
            .Where(l => l.Id == request.Id)
            .Select(l => new ServiceListingDetailDto
            {
                Id = l.Id,
                ProviderProfileId = l.ProviderProfileId,
                CategoryId = l.CategoryId,
                Title = l.Title,
                Description = l.Description,
                Tags = l.Tags,
                EquipmentModel = l.EquipmentModel,
                EquipmentCapacity = l.EquipmentCapacity,
                EquipmentCondition = l.EquipmentCondition,
                YearOfManufacture = l.YearOfManufacture,
                EquipmentRegistrationNumber = l.EquipmentRegistrationNumber,
                HourlyRate = l.HourlyRate,
                DailyRate = l.DailyRate,
                MinimumBookingHours = l.MinimumBookingHours,
                Status = l.Status,
                QualityScore = l.QualityScore,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt,
                Photos = l.Photos
                    .OrderBy(p => p.DisplayOrder)
                    .Select(p => new ServiceListingPhotoDto
                    {
                        Id = p.Id,
                        ListingId = p.ListingId,
                        PhotoUrl = p.PhotoUrl,
                        DisplayOrder = p.DisplayOrder
                    }).ToList(),
                Operators = l.ServiceListingOperators
                    .Select(slo => new ServiceListingOperatorDto
                    {
                        OperatorId = slo.Operator.Id,
                        FullName = slo.Operator.FullName,
                        YearsOfExperience = slo.Operator.YearsOfExperience,
                        Specialization = slo.Operator.Specialization,
                        LicenseType = slo.Operator.LicenseType
                    }).ToList(),
       
                Availability = l.Availability
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
                    )).ToList()
            }) 
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(ServiceListing), request.Id);

        return listing;
    }
}