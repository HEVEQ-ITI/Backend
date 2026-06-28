using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEVEQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGovernorateAndRegionToServiceListing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Governorate",
                table: "ServiceListings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "ServiceListings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Governorate",
                table: "ServiceListings");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "ServiceListings");
        }
    }
}
