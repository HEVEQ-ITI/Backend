using Hangfire;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Common.Jobs;
using HEVEQ.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HEVEQ.Infrastructure.Services.BackgroundJobs
{
    public class CustomerCompletionAutoConfirmJob
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CustomerCompletionAutoConfirmJob> _logger;
        private readonly BackgroundJobOptions _options;

        public CustomerCompletionAutoConfirmJob(IApplicationDbContext context, ILogger<CustomerCompletionAutoConfirmJob> logger, IOptions<BackgroundJobOptions> options)
        {
            _context = context;
            _logger = logger;
            _options = options.Value;
        }

        [DisableConcurrentExecution(timeoutInSeconds: 600)]
        public async Task RunAsync()
        {
            var cutoff = DateTime.UtcNow.AddHours(-_options.CustomerCompletionAutoConfirmHours);

            var bookings = await _context.Bookings
                .Where(x =>
                    x.Status == BookingStatus.PendingCustomerConfirmation &&
                    x.CompletedMarkedAt != null &&
                    x.CompletedMarkedAt <= cutoff &&
                    x.DisputeOpenedAt == null)
                .ToListAsync();

            if (bookings.Count == 0)
            {
                _logger.LogInformation("CustomerCompletionAutoConfirmJob: no bookings found.");
                return;
            }

            var bookingIds = bookings.Select(x => x.Id).ToList();

            var pendingAdjustments = await _context.BookingTimeAdjustmentRequests
                .Where(x =>
                    bookingIds.Contains(x.BookingId) &&
                    x.Status == BookingTimeAdjustmentStatus.Pending)
                .ToListAsync();

            foreach (var booking in bookings)
            {
                booking.Status = BookingStatus.Completed;
                booking.CompletionConfirmedAt = DateTime.UtcNow;

                // TODO: Blocked by Developer 2 NotificationCreationService.
                // Create notification for customer and provider when booking is auto-confirmed.
            }

            foreach (var adjustment in pendingAdjustments)
            {
                adjustment.Status = BookingTimeAdjustmentStatus.Expired;
            }

            await _context.SaveChangesAsync(CancellationToken.None);

            _logger.LogInformation("CustomerCompletionAutoConfirmJob: auto-confirmed {Count} bookings.", bookings.Count);
        }
    }
}