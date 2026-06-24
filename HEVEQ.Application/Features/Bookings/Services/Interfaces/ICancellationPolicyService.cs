using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Services.Interfaces
{
    public interface ICancellationPolicyService
    {
        Task<BookingCancellationInitiator> ResolveActorAsync(Booking booking, Guid userId, CancellationToken cancellationToken);
        decimal CalculateRefundPercentage(BookingStatus status);
        void EnsureCanBeCancelled(BookingStatus status);
    }
}
