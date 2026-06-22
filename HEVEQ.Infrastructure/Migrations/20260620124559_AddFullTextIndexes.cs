using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEVEQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFullTextIndexes : Migration
    {
        /// <inheritdoc />
        //protected override void Up(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.Sql("""
        //        IF FULLTEXTSERVICEPROPERTY('IsFullTextInstalled') <> 1
        //        BEGIN
        //            THROW 51000, 'Full-Text Search is not installed on this SQL Server instance.', 1;
        //        END
        //    """, suppressTransaction: true);

        //    migrationBuilder.Sql("""
        //        IF NOT EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE name = N'ShareGearFTSCatalog')
        //        BEGIN
        //            CREATE FULLTEXT CATALOG [ShareGearFTSCatalog] AS DEFAULT;
        //        END
        //    """, suppressTransaction: true);

        //    migrationBuilder.Sql("""
        //        IF NOT EXISTS (
        //            SELECT 1
        //            FROM sys.fulltext_indexes
        //            WHERE object_id = OBJECT_ID(N'[dbo].[ServiceListings]')
        //        )
        //        BEGIN
        //            CREATE FULLTEXT INDEX ON [dbo].[ServiceListings]
        //            (
        //                [Title] LANGUAGE 1025,
        //                [Description] LANGUAGE 1025
        //            )
        //            KEY INDEX [PK_ServiceListings]
        //            ON [ShareGearFTSCatalog]
        //            WITH CHANGE_TRACKING AUTO, STOPLIST = SYSTEM;
        //        END
        //    """, suppressTransaction: true);

        //    migrationBuilder.Sql("""
        //        IF NOT EXISTS (
        //            SELECT 1
        //            FROM sys.fulltext_indexes
        //            WHERE object_id = OBJECT_ID(N'[dbo].[MarketplaceListings]')
        //        )
        //        BEGIN
        //            CREATE FULLTEXT INDEX ON [dbo].[MarketplaceListings]
        //            (
        //                [Title] LANGUAGE 1025,
        //                [Description] LANGUAGE 1025
        //            )
        //            KEY INDEX [PK_MarketplaceListings]
        //            ON [ShareGearFTSCatalog]
        //            WITH CHANGE_TRACKING AUTO, STOPLIST = SYSTEM;
        //        END
        //    """, suppressTransaction: true);
        //}


        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // عملنا إيقاف كامل لكل أوامر الـ Full-Text لأن الـ LocalDB لا يدعمها نهائياً
            migrationBuilder.Sql("PRINT 'Skipping all Full-Text operations for LocalDB';");

            /*
            migrationBuilder.Sql("""
                IF FULLTEXTSERVICEPROPERTY('IsFullTextInstalled') <> 1
                BEGIN
                    PRINT 'Skipping full text check for now';
                END
            """, suppressTransaction: true);

            migrationBuilder.Sql("""
                IF NOT EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE name = N'ShareGearFTSCatalog')
                BEGIN
                    CREATE FULLTEXT CATALOG [ShareGearFTSCatalog] AS DEFAULT;
                END
            """, suppressTransaction: true);

            migrationBuilder.Sql("""
                IF NOT EXISTS (
                    SELECT 1
                    FROM sys.fulltext_indexes
                    WHERE object_id = OBJECT_ID(N'[dbo].[ServiceListings]')
                )
                BEGIN
                    CREATE FULLTEXT INDEX ON [dbo].[ServiceListings]
                    (
                        [Title] LANGUAGE 1025,
                        [Description] LANGUAGE 1025
                    )
                    KEY INDEX [PK_ServiceListings]
                    ON [ShareGearFTSCatalog]
                    WITH CHANGE_TRACKING AUTO, STOPLIST = SYSTEM;
                END
            """, suppressTransaction: true);

            migrationBuilder.Sql("""
                IF NOT EXISTS (
                    SELECT 1
                    FROM sys.fulltext_indexes
                    WHERE object_id = OBJECT_ID(N'[dbo].[MarketplaceListings]')
                )
                BEGIN
                    CREATE FULLTEXT INDEX ON [dbo].[MarketplaceListings]
                    (
                        [Title] LANGUAGE 1025,
                        [Description] LANGUAGE 1025
                    )
                    KEY INDEX [PK_MarketplaceListings]
                    ON [ShareGearFTSCatalog]
                    WITH CHANGE_TRACKING AUTO, STOPLIST = SYSTEM;
                END
            """, suppressTransaction: true);
            */
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF EXISTS (
                    SELECT 1
                    FROM sys.fulltext_indexes
                    WHERE object_id = OBJECT_ID(N'[dbo].[ServiceListings]')
                )
                BEGIN
                    DROP FULLTEXT INDEX ON [dbo].[ServiceListings];
                END
            """, suppressTransaction: true);

            migrationBuilder.Sql("""
                IF EXISTS (
                    SELECT 1
                    FROM sys.fulltext_indexes
                    WHERE object_id = OBJECT_ID(N'[dbo].[MarketplaceListings]')
                )
                BEGIN
                    DROP FULLTEXT INDEX ON [dbo].[MarketplaceListings];
                END
            """, suppressTransaction: true);

            migrationBuilder.Sql("""
                IF EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE name = N'ShareGearFTSCatalog')
                BEGIN
                    DROP FULLTEXT CATALOG [ShareGearFTSCatalog];
                END
            """, suppressTransaction: true);
        }
    }
}