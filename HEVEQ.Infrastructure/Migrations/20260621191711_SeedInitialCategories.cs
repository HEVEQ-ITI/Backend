using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HEVEQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF EXISTS (
                    SELECT 1
                    FROM [sys].[identity_columns]
                    WHERE [name] = N'Id'
                    AND [object_id] = OBJECT_ID(N'[Categories]')
            )
    BEGIN
    SET IDENTITY_INSERT [Categories] ON;
END

IF NOT EXISTS (
    SELECT 1 FROM [Categories]
    WHERE [Id] = 1 OR [Slug] = N'heavy-excavators'
)
BEGIN
    INSERT INTO [Categories] ([Id], [Name], [ParentId], [Slug], [Type])
    VALUES (1, N'Heavy Excavators', NULL, N'heavy-excavators', 0);
END

IF NOT EXISTS (
    SELECT 1 FROM [Categories]
    WHERE [Id] = 2 OR [Slug] = N'tower-cranes'
)
BEGIN
    INSERT INTO [Categories] ([Id], [Name], [ParentId], [Slug], [Type])
    VALUES (2, N'Tower Cranes', NULL, N'tower-cranes', 0);
END

IF NOT EXISTS (
    SELECT 1 FROM [Categories]
    WHERE [Id] = 3 OR [Slug] = N'concrete-mixers'
)
BEGIN
    INSERT INTO [Categories] ([Id], [Name], [ParentId], [Slug], [Type])
    VALUES (3, N'Concrete Mixers', NULL, N'concrete-mixers', 0);
END

IF EXISTS (
    SELECT 1
    FROM [sys].[identity_columns]
    WHERE [name] = N'Id'
      AND [object_id] = OBJECT_ID(N'[Categories]')
)
BEGIN
    SET IDENTITY_INSERT [Categories] OFF;
END
""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
            DELETE FROM [Categories]
            WHERE [Id] IN (1, 2, 3)
                AND [Slug] IN (
                N'heavy-excavators',
                N'tower-cranes',
                N'concrete-mixers'
            );
        """);
        }
    }
}
