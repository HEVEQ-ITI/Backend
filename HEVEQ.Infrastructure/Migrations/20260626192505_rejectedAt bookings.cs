using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEVEQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class rejectedAtbookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedAt",
                table: "Bookings",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectedAt",
                table: "Bookings");
        }
    }
}
