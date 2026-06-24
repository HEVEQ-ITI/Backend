using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Bookings.Services.Interfaces;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Services.Implementation
{
    public sealed class CancellationPolicyService : ICancellationPolicyService
    {
        private readonly IApplicationDbContext _context;

        public CancellationPolicyService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BookingCancellationInitiator> ResolveActorAsync(
            Booking booking,
            Guid userId,
            CancellationToken cancellationToken)
        {
            var isCustomer = booking.CustomerId == userId;

            var providerProfile = await _context.ProviderProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            var isProviderOwner = providerProfile is not null &&
                booking.ServiceListing.ProviderProfileId == providerProfile.Id;

            if (isCustomer && isProviderOwner)
                throw new InvalidOperationException("User cannot cancel as both customer and provider.");

            if (isCustomer) return BookingCancellationInitiator.Customer;
            if (isProviderOwner) return BookingCancellationInitiator.Provider;

            throw new InvalidOperationException("You are not allowed to cancel this booking.");
        }

        public decimal CalculateRefundPercentage(BookingStatus status)
        {
            return status switch
            {
                BookingStatus.PendingProviderResponse => 100m,
                BookingStatus.ConfirmedPendingPayment => 100m,
                BookingStatus.Active => 100m,
                _ => 0m
            };
        }

        public void EnsureCanBeCancelled(BookingStatus status)
        {
            if (status == BookingStatus.Cancelled)
                throw new InvalidOperationException("Booking is already cancelled.");

            if (status == BookingStatus.Rejected)
                throw new InvalidOperationException("Rejected bookings cannot be cancelled.");

            if (status == BookingStatus.InProgress)
                throw new InvalidOperationException("Booking cannot be cancelled after it has started.");

            if (status == BookingStatus.PendingCustomerConfirmation)
                throw new InvalidOperationException("Booking cannot be cancelled after provider marked it as completed.");

            if (status == BookingStatus.Completed)
                throw new InvalidOperationException("Completed bookings cannot be cancelled.");

            if (status == BookingStatus.Disputed)
                throw new InvalidOperationException("Disputed bookings cannot be cancelled.");
        }
    }
}
