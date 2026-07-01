using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.Helpers;
using HEVEQ.Application.Features.ProviderCalendar.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HEVEQ.Application.Features.ProviderCalendar.Queries.GetProviderCalendar;

public class GetProviderCalendarQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetProviderCalendarQuery, ProviderCalendarResultDto>
{
    public async Task<ProviderCalendarResultDto> Handle(GetProviderCalendarQuery request, CancellationToken cancellationToken)
    {
        if (!currentUserService.UserId.HasValue)
            throw new ForbiddenAccessException("User is not authenticated.");

        var providerProfileId = await context.ProviderProfiles
            .AsNoTracking()
            .Where(p => p.UserId == currentUserService.UserId.Value)
            .Select(p => p.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (providerProfileId == Guid.Empty)
            throw new ForbiddenAccessException("Only registered providers can access the calendar.");

        var listingIds = await context.ServiceListings
            .AsNoTracking()
            .Where(l => l.ProviderProfileId == providerProfileId)
            .Select(l => l.Id)
            .ToListAsync(cancellationToken);

        var rangeStart = request.From.ToDateTime(TimeOnly.MinValue);
        var rangeEndExclusive = request.To.ToDateTime(TimeOnly.MaxValue);

        // Scheduled bookings — only assignments that actually overlap the
        // requested window. Ownership enforced via listingIds, never via a
        // client-supplied providerId.
        var bookingItems = await context.OperatorAssignments
            .AsNoTracking()
            .Where(oa => listingIds.Contains(oa.Booking.ServiceListingId)
                      && oa.ScheduledStart < rangeEndExclusive
                      && oa.ScheduledEnd > rangeStart)
            .Select(oa => new CalendarItemDto(
                "Booking",
                oa.BookingId,
                null,
                (oa.Booking.BookingNumber ?? "BK-" + oa.BookingId.ToString().Substring(0, 4)) + " - " + oa.Booking.ServiceListing.Title,
                oa.Operator != null ? oa.Operator.FullName : "No Operator Assigned",
                oa.ScheduledStart,
                oa.ScheduledEnd,
                null,
                oa.Booking.Status.ToString(),
                oa.Booking.Status.GetStatusAr()
            ))
            .ToListAsync(cancellationToken);

        // Blackout dates for the same provider's listings within the window.
        var blackoutItems = await context.BlackoutDates
            .AsNoTracking()
            .Where(b => listingIds.Contains(b.ListingId)
                     && b.Date >= request.From
                     && b.Date <= request.To)
            .Select(b => new CalendarItemDto(
                "BlackoutDate",
                null,
                b.ListingId,
                b.Reason ?? "Blackout", // defensive null-guard — never null/blank on the wire
                null,
                null,
                null,
                b.Date,
                null,
                null
            ))
            .ToListAsync(cancellationToken);

        var items = new List<CalendarItemDto>();
        items.AddRange(bookingItems);
        items.AddRange(blackoutItems);

        return new ProviderCalendarResultDto(items);
    }
}