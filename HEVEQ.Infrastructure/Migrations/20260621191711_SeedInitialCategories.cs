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
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "ParentId", "Slug", "Type" },
                values: new object[,]
                {
                    { 1, "Heavy Excavators", null, "heavy-excavators", 0 },
                    { 2, "Tower Cranes", null, "tower-cranes", 0 },
                    { 3, "Concrete Mixers", null, "concrete-mixers", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
