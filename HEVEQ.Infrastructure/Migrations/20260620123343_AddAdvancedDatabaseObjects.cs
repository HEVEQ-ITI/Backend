using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEVEQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAdvancedDatabaseObjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'SIX_ProviderProfiles_ServiceZoneCenter')
        BEGIN
            CREATE SPATIAL INDEX [SIX_ProviderProfiles_ServiceZoneCenter]
            ON [dbo].[ProviderProfiles]([ServiceZoneCenter])
            USING GEOGRAPHY_AUTO_GRID
            WITH (CELLS_PER_OBJECT = 16);
        END
    """);

            migrationBuilder.Sql("""
        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'SIX_Bookings_ServiceLocationGeo')
        BEGIN
            CREATE SPATIAL INDEX [SIX_Bookings_ServiceLocationGeo]
            ON [dbo].[Bookings]([ServiceLocationGeo])
            USING GEOGRAPHY_AUTO_GRID
            WITH (CELLS_PER_OBJECT = 16);
        END
    """);

            migrationBuilder.Sql("""
        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_ProviderProfiles_AlternativeSearch')
        BEGIN
            CREATE INDEX [IX_ProviderProfiles_AlternativeSearch]
            ON [dbo].[ProviderProfiles]
            (
                [ResponseRate] DESC,
                [AverageRating] DESC,
                [OnboardingTier] DESC
            )
            INCLUDE
            (
                [UserId],
                [CompanyName],
                [BaseLatitude],
                [BaseLongitude],
                [ServiceRadiusKm],
                [SearchRankingModifier],
                [TrustLevel]
            )
            WHERE [ResponseRate] >= 0.80;
        END
    """);

            migrationBuilder.Sql("""
        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_TimeAdj_BookingId_Pending')
        BEGIN
            CREATE UNIQUE INDEX [UX_TimeAdj_BookingId_Pending]
            ON [dbo].[BookingTimeAdjustmentRequests]([BookingId])
            WHERE [Status] = 0;
        END
    """);

            migrationBuilder.Sql("""
        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_TimeAdj_BookingId')
        BEGIN
            CREATE INDEX [IX_TimeAdj_BookingId]
            ON [dbo].[BookingTimeAdjustmentRequests]([BookingId], [Status]);
        END
    """);

            migrationBuilder.Sql("""
        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_EscrowRecords_BookingId')
        BEGIN
            CREATE UNIQUE INDEX [UX_EscrowRecords_BookingId]
            ON [dbo].[EscrowRecords]([BookingId])
            WHERE [BookingId] IS NOT NULL;
        END
    """);

            migrationBuilder.Sql("""
        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_EscrowRecords_OrderId')
        BEGIN
            CREATE UNIQUE INDEX [UX_EscrowRecords_OrderId]
            ON [dbo].[EscrowRecords]([MarketplaceOrderId])
            WHERE [MarketplaceOrderId] IS NOT NULL;
        END
    """);

            migrationBuilder.Sql("""
        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SearchQueryLogs_CreatedAt')
        BEGIN
            CREATE INDEX [IX_SearchQueryLogs_CreatedAt]
            ON [dbo].[SearchQueryLogs]([CreatedAt])
            INCLUDE
            (
                [ContextDomain],
                [SearchMode],
                [ResultCount],
                [HasLowConfidence],
                [HasZeroResults],
                [AlternativeSuggested],
                [ProcessingMs]
            );
        END
    """);

            migrationBuilder.Sql("""
        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SearchQueryLogs_UserId')
        BEGIN
            CREATE INDEX [IX_SearchQueryLogs_UserId]
            ON [dbo].[SearchQueryLogs]([UserId], [CreatedAt] DESC)
            WHERE [UserId] IS NOT NULL;
        END
    """);

            migrationBuilder.Sql("""
        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_DomainEventQueue_Processing')
        BEGIN
            CREATE INDEX [IX_DomainEventQueue_Processing]
            ON [dbo].[DomainEventQueue]([Status], [RetryCount], [CreatedAt])
            INCLUDE ([EventType], [EntityType], [EntityId]);
        END
    """);

            migrationBuilder.Sql("""
        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_OperatorAssignments_ConflictDetection')
        BEGIN
            CREATE INDEX [IX_OperatorAssignments_ConflictDetection]
            ON [dbo].[OperatorAssignments]([OperatorId], [ScheduledStart], [ScheduledEnd])
            INCLUDE ([BookingId], [Status]);
        END
    """);

            migrationBuilder.Sql("""
        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_Conversations_ServiceListing_Participants')
        BEGIN
            CREATE UNIQUE INDEX [UX_Conversations_ServiceListing_Participants]
            ON [dbo].[Conversations]([ServiceListingId], [InitiatedById], [ParticipantId])
            WHERE [ServiceListingId] IS NOT NULL;
        END
    """);

            migrationBuilder.Sql("""
        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Conversations_ParticipantId')
        BEGIN
            CREATE INDEX [IX_Conversations_ParticipantId]
            ON [dbo].[Conversations]([ParticipantId], [IsLocked]);
        END
    """);

            migrationBuilder.Sql("""
        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Reviews_ReviewedUserId')
        BEGIN
            CREATE INDEX [IX_Reviews_ReviewedUserId]
            ON [dbo].[Reviews]([ReviewedUserId], [IsPublished]);
        END
    """);

            migrationBuilder.Sql("""
        IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_ServiceListingAvailability_TimeRange')
        BEGIN
            ALTER TABLE [dbo].[ServiceListingAvailability]
            ADD CONSTRAINT [CK_ServiceListingAvailability_TimeRange]
            CHECK ([CloseTime] > [OpenTime]);
        END
    """);

            migrationBuilder.Sql("""
        IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_MarketplaceOrders_Status')
        BEGIN
            ALTER TABLE [dbo].[MarketplaceOrders]
            ADD CONSTRAINT [CK_MarketplaceOrders_Status]
            CHECK ([Status] BETWEEN 0 AND 9);
        END
    """);

            migrationBuilder.Sql("""
        IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_Bookings_NoSelfReassign')
        BEGIN
            ALTER TABLE [dbo].[Bookings]
            ADD CONSTRAINT [CK_Bookings_NoSelfReassign]
            CHECK
            (
                [ReassignedToBookingId] IS NULL
                OR [OriginalBookingId] IS NULL
                OR [ReassignedToBookingId] <> [OriginalBookingId]
            );
        END
    """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP INDEX IF EXISTS [SIX_ProviderProfiles_ServiceZoneCenter] ON [dbo].[ProviderProfiles];");
            migrationBuilder.Sql("DROP INDEX IF EXISTS [SIX_Bookings_ServiceLocationGeo] ON [dbo].[Bookings];");
            migrationBuilder.Sql("DROP INDEX IF EXISTS [IX_ProviderProfiles_AlternativeSearch] ON [dbo].[ProviderProfiles];");

            migrationBuilder.Sql("DROP INDEX IF EXISTS [UX_TimeAdj_BookingId_Pending] ON [dbo].[BookingTimeAdjustmentRequests];");
            migrationBuilder.Sql("DROP INDEX IF EXISTS [IX_TimeAdj_BookingId] ON [dbo].[BookingTimeAdjustmentRequests];");

            migrationBuilder.Sql("DROP INDEX IF EXISTS [UX_EscrowRecords_BookingId] ON [dbo].[EscrowRecords];");
            migrationBuilder.Sql("DROP INDEX IF EXISTS [UX_EscrowRecords_OrderId] ON [dbo].[EscrowRecords];");

            migrationBuilder.Sql("DROP INDEX IF EXISTS [IX_SearchQueryLogs_CreatedAt] ON [dbo].[SearchQueryLogs];");
            migrationBuilder.Sql("DROP INDEX IF EXISTS [IX_SearchQueryLogs_UserId] ON [dbo].[SearchQueryLogs];");

            migrationBuilder.Sql("DROP INDEX IF EXISTS [IX_DomainEventQueue_Processing] ON [dbo].[DomainEventQueue];");
            migrationBuilder.Sql("DROP INDEX IF EXISTS [IX_OperatorAssignments_ConflictDetection] ON [dbo].[OperatorAssignments];");

            migrationBuilder.Sql("DROP INDEX IF EXISTS [UX_Conversations_ServiceListing_Participants] ON [dbo].[Conversations];");
            migrationBuilder.Sql("DROP INDEX IF EXISTS [IX_Conversations_ParticipantId] ON [dbo].[Conversations];");

            migrationBuilder.Sql("DROP INDEX IF EXISTS [IX_Reviews_ReviewedUserId] ON [dbo].[Reviews];");

            migrationBuilder.Sql("ALTER TABLE [dbo].[ServiceListingAvailability] DROP CONSTRAINT IF EXISTS [CK_ServiceListingAvailability_TimeRange];");
            migrationBuilder.Sql("ALTER TABLE [dbo].[MarketplaceOrders] DROP CONSTRAINT IF EXISTS [CK_MarketplaceOrders_Status];");
            migrationBuilder.Sql("ALTER TABLE [dbo].[Bookings] DROP CONSTRAINT IF EXISTS [CK_Bookings_NoSelfReassign];");
        }
    }
}
