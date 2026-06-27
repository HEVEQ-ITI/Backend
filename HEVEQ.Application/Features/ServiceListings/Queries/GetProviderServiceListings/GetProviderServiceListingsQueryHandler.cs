//using HEVEQ.Application.Common.Exceptions;
//using HEVEQ.Application.Common.Extensions;
//using HEVEQ.Application.Common.Interfaces;
//using HEVEQ.Application.Features.ServiceListings.DTOs;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace HEVEQ.Application.Features.ServiceListings.Queries.GetProviderServiceListings
//{
//    public class GetProviderServiceListingsQueryHandler(
//        IApplicationDbContext context,
//        ICurrentUserService currentUserService)
//        : IRequestHandler<GetProviderServiceListingsQuery, ProviderServiceListingsResultDto>
//    {
//        public async Task<ProviderServiceListingsResultDto> Handle(
//            GetProviderServiceListingsQuery request, CancellationToken cancellationToken)
//        {
//            // Pillar 1 — Dynamic Identity Resolution.
//            // Guid.TryParse handles a null UserId gracefully (returns false), so a
//            // missing/garbled claim fails closed here instead of throwing a raw
//            // FormatException further down or silently resolving to Guid.Empty.
//            var userId = currentUserService.UserId;

//            if (!userId.HasValue)
//                throw new ForbiddenAccessException("User is not authenticated.");

//            var providerProfileId = await context.ProviderProfiles.AsNoTracking()
//                .Where(p => p.UserId == userId.Value)
//                .Select(p => p.Id)
//                .SingleOrDefaultAsync(cancellationToken);

//            if (providerProfileId == Guid.Empty)
//                throw new ForbiddenAccessException("No provider profile is registered for the current user.");

//            var query = context.ServiceListings
//                .AsNoTracking()
//                .Where(l => l.ProviderProfileId == providerProfileId);

//            if (request.Status is not null)
//                query = query.Where(l => l.Status == request.Status);

//            // Pillar 4 — UI-Enriched DTOs.
//            // CategoryName comes from a direct navigation-property access (EF Core
//            // translates this into a SQL JOIN). PhotosCount / OperatorsCount /
//            // AvailabilityCount come from .Count() on the collection navigations
//            // (EF Core translates each into a correlated subquery). CoverPhotoUrl
//            // pulls the lowest-DisplayOrder photo. All of this happens in one
//            // database round trip — nothing here is client-evaluated yet.
//            var rawItems = await query
//                .OrderByDescending(l => l.CreatedAt)
//                .Select(l => new
//                {
//                    l.Id,
//                    l.Title,
//                    CategoryName = l.Category!.Name,
//                    CoverPhotoUrl = l.Photos
//                        .OrderBy(p => p.DisplayOrder)
//                        .Select(p => p.PhotoUrl)
//                        .FirstOrDefault(),
//                    l.HourlyRate,
//                    l.Status,
//                    l.QualityScore,
//                    PhotosCount = l.Photos.Count,
//                    OperatorsCount = l.ServiceListingOperators.Count,
//                    AvailabilityCount = l.Availability.Count,
//                    l.CreatedAt
//                })
//                .ToListAsync(cancellationToken);

//            // Pillar 5 — Dual-Language Localization.
//            // ToArabicText() is a plain C# switch expression — it cannot be
//            // translated to SQL. It's deliberately applied here, in-memory, on the
//            // already-materialized page of results, rather than inside the
//            // .Select() above (which would throw at query-translation time).
//            var items = rawItems
//                .Select(l => new ProviderServiceListingDto
//                {
//                    Id = l.Id,
//                    Title = l.Title,
//                    CategoryName = l.CategoryName,
//                    CoverPhotoUrl = l.CoverPhotoUrl,
//                    HourlyRate = (decimal)l.HourlyRate,
//                    Status = l.Status.ToString(),
//                    StatusAr = l.Status.ToArabicText(),
//                    QualityScore = (int)l.QualityScore,
//                    PhotosCount = l.PhotosCount,
//                    OperatorsCount = l.OperatorsCount,
//                    AvailabilityCount = l.AvailabilityCount,
//                    CreatedAt = l.CreatedAt
//                })
//                .ToList();

//            return new ProviderServiceListingsResultDto
//            {
//                Items = items,
//                TotalCount = items.Count
//            };
//        }
//    }
//}


using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Extensions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.ServiceListings.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HEVEQ.Application.Features.ServiceListings.Queries.GetProviderServiceListings;

// 1️⃣ أضفنا الـ Query المفقود هنا عشان الـ Handler والـ Controller يشوفوه فوراً
//public record GetProviderServiceListingsQuery(ServiceListingStatus? Status = null) : IRequest<ProviderServiceListingsResultDto>;

public class GetProviderServiceListingsQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetProviderServiceListingsQuery, ProviderServiceListingsResultDto>
{
    public async Task<ProviderServiceListingsResultDto> Handle(
        GetProviderServiceListingsQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        if (!userId.HasValue)
            throw new ForbiddenAccessException("User is not authenticated.");

        var providerProfileId = await context.ProviderProfiles.AsNoTracking()
            .Where(p => p.UserId == userId.Value)
            .Select(p => p.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (providerProfileId == Guid.Empty)
            throw new ForbiddenAccessException("No provider profile is registered for the current user.");

        var query = context.ServiceListings
            .AsNoTracking()
            .Where(l => l.ProviderProfileId == providerProfileId);

        if (request.Status is not null)
            query = query.Where(l => l.Status == request.Status.Value);

        var rawItems = await query
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new
            {
                l.Id,
                l.Title,
                CategoryName = l.Category!.Name,
                CoverPhotoUrl = l.Photos
                    .OrderBy(p => p.DisplayOrder)
                    .Select(p => p.PhotoUrl)
                    .FirstOrDefault(),
                l.HourlyRate,
                l.Status,
                l.QualityScore,
                PhotosCount = l.Photos.Count,
                OperatorsCount = l.ServiceListingOperators.Count,
                AvailabilityCount = l.Availability.Count,
                l.CreatedAt
            })
            .ToListAsync(cancellationToken);

        // 2️⃣ هنا صلّحنا الـ Mapping وباصينا القيم جوه الـ Constructor بالترتيب المطلوب للـ Record
        var items = rawItems
            .Select(l => new ProviderServiceListingDto(
                l.Id,
                l.Title,
                l.CategoryName,
                l.CoverPhotoUrl,
                (decimal)l.HourlyRate,
                l.Status.ToString(),
                l.Status.ToArabicText(),
                (int)l.QualityScore,
                l.PhotosCount,
                l.OperatorsCount,
                l.AvailabilityCount,
                l.CreatedAt
            ))
            .ToList();

        return new ProviderServiceListingsResultDto
        {
            Items = items,
            TotalCount = items.Count
        };
    }
}