using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEVEQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderNumber",
                table: "MarketplaceOrders",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.Sql(@"
UPDATE MarketplaceOrders
SET OrderNumber = CONCAT(
    'OR-',
    FORMAT(GETUTCDATE(), 'yyyyMMdd'),
    '-',
    UPPER(LEFT(REPLACE(CONVERT(nvarchar(36), Id), '-', ''), 6))
)
WHERE OrderNumber IS NULL OR OrderNumber = ''
");

            migrationBuilder.AlterColumn<string>(
                name: "OrderNumber",
                table: "MarketplaceOrders",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MarketplaceOrders_OrderNumber",
                table: "MarketplaceOrders",
                column: "OrderNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MarketplaceOrders_OrderNumber",
                table: "MarketplaceOrders");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "MarketplaceOrders");
        }
    }
}
