using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Application.Features.Bookings.Helpers;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.CreateTimeAdjustment
{
    public sealed class CreateTimeAdjustmentCommandHandler : IRequestHandler<CreateTimeAdjustmentCommand, CreateTimeAdjustmentResponseDto>
    {
        private readonly IApplicationDbContext _context;

        public CreateTimeAdjustmentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateTimeAdjustmentResponseDto> Handle(CreateTimeAdjustmentCommand request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings
                .Include(x => x.ServiceListing)
                .FirstOrDefaultAsync(x => x.Id == request.BookingId, cancellationToken);

            if (booking is null)
                throw new InvalidOperationException("Booking was not found.");

            if (booking.Status != BookingStatus.Active && booking.Status != BookingStatus.InProgress)
                throw new InvalidOperationException("Time adjustment can only be requested for active or in-progress bookings.");

            var providerProfile = await _context.ProviderProfiles
                .FirstOrDefaultAsync(x => x.UserId == request.ProviderUserId, cancellationToken);

            if (providerProfile is null)
                throw new InvalidOperationException("Provider profile was not found.");

            if (booking.ServiceListing.ProviderProfileId != providerProfile.Id)
                throw new InvalidOperationException("Only the owner provider can request time adjustment for this booking.");

            var hasPendingRequest = await _context.BookingTimeAdjustmentRequests
                .AnyAsync(x =>
                    x.BookingId == booking.Id &&
                    x.Status == BookingTimeAdjustmentStatus.Pending,
                    cancellationToken);

            if (hasPendingRequest)
                throw new InvalidOperationException("There is already a pending time adjustment request for this booking.");

            var additionalCostAmount = booking.HourlyRateSnapshot * request.AdditionalHours;

            var adjustmentRequest = new BookingTimeAdjustmentRequest
            {
                BookingId = booking.Id,
                RequestedAdditionalHrs = request.AdditionalHours,
                AdditionalCostAmount = additionalCostAmount,
                Status = BookingTimeAdjustmentStatus.Pending,
                ProviderNote = request.Reason.Trim(),
                CreatedAt = DateTime.Now
            };

            _context.BookingTimeAdjustmentRequests.Add(adjustmentRequest);
            await _context.SaveChangesAsync(cancellationToken);
            return new CreateTimeAdjustmentResponseDto
            {
                Id = adjustmentRequest.Id,
                BookingId = booking.Id,
                BookingNumber = booking.BookingNumber,
                RequestedAdditionalHours = adjustmentRequest.RequestedAdditionalHrs,
                AdditionalCostAmount = adjustmentRequest.AdditionalCostAmount,
                Status = adjustmentRequest.Status.ToString(),
                StatusAr = TimeAdjustmentDisplayHelper.GetStatusAr(adjustmentRequest.Status),
                ProviderNote = adjustmentRequest.ProviderNote ?? string.Empty,
                Message = "Time adjustment request created successfully"
            };
        }
    }
}
