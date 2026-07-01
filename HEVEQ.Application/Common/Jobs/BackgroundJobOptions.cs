namespace HEVEQ.Application.Common.Jobs
{
    public class BackgroundJobOptions
    {
        public int ProviderResponseSlaHours { get; set; } = 6;
        public int CustomerCompletionAutoConfirmHours { get; set; } = 24;
        public int EscrowReleaseAfterCompletionHours { get; set; } = 48;

        public int MarketplaceAutoConfirmHours { get; set; } = 24;
        public int MarketplaceEscrowReleaseHours { get; set; } = 48;

        public string ProviderResponseSlaCron { get; set; } = "*/15 * * * *";
        public string CustomerCompletionAutoConfirmCron { get; set; } = "*/15 * * * *";
        public string EscrowReleaseAfterCompletionCron { get; set; } = "*/15 * * * *";

        public string MarketplaceAutoConfirmCron { get; set; } = "*/15 * * * *";
        public string MarketplaceEscrowReleaseCron { get; set; } = "*/15 * * * *";
    }
}