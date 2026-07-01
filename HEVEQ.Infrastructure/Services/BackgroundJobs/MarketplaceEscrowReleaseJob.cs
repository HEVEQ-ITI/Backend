using Hangfire;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Common.Jobs;
using HEVEQ.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HEVEQ.Infrastructure.Services.BackgroundJobs
{
    public class MarketplaceEscrowReleaseJob
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<MarketplaceEscrowReleaseJob> _logger;
        private readonly BackgroundJobOptions _options;

        public MarketplaceEscrowReleaseJob(IApplicationDbContext context, ILogger<MarketplaceEscrowReleaseJob> logger, IOptions<BackgroundJobOptions> options)
        {
            _context = context;
            _logger = logger;
            _options = options.Value;
        }

        [DisableConcurrentExecution(timeoutInSeconds: 600)]
        public async Task RunAsync()
        {
            var now = DateTime.UtcNow;
            var fallbackCutoff = now.AddHours(-_options.MarketplaceEscrowReleaseHours);

            var escrows = await _context.EscrowRecords
                .Include(x => x.MarketplaceOrder)
                .Where(x =>
                    x.MarketplaceOrderId != null &&
                    x.Status == EscrowStatus.Held &&
                    x.FrozenAt == null &&
                    x.MarketplaceOrder != null &&
                    x.MarketplaceOrder.Status == MarketplaceOrderStatus.Completed &&
                    x.MarketplaceOrder.ConfirmedByBuyerAt != null &&
                    x.MarketplaceOrder.ConfirmedByBuyerAt <= fallbackCutoff)
                .ToListAsync();

            if (escrows.Count == 0)
            {
                _logger.LogInformation("MarketplaceEscrowReleaseJob: no eligible marketplace escrow records found.");
                return;
            }

            var releasedCount = 0;

            foreach (var escrow in escrows)
            {
                if (escrow.EarliestReleaseAt is not null &&
                    escrow.EarliestReleaseAt > now)
                {
                    continue;
                }

                escrow.Status = EscrowStatus.Released;
                escrow.ReleasedAt = now;

                releasedCount++;

                // TODO: Blocked by Developer 2 NotificationCreationService.
                // Create notification for seller when marketplace escrow is released.
            }

            if (releasedCount == 0)
            {
                _logger.LogInformation("MarketplaceEscrowReleaseJob: no marketplace escrow records released.");
                return;
            }

            await _context.SaveChangesAsync(CancellationToken.None);

            _logger.LogInformation(
                "MarketplaceEscrowReleaseJob: released {Count} marketplace escrow records.",
                releasedCount);
        }
    }
}