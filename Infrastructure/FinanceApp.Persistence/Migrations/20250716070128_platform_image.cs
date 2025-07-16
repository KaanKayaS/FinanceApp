using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class platform_image : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "DigitalPlatforms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ImagePath" },
                values: new object[] { new DateTime(2025, 7, 16, 13, 1, 27, 984, DateTimeKind.Local).AddTicks(5495), null });

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ImagePath" },
                values: new object[] { new DateTime(2025, 7, 16, 13, 1, 27, 984, DateTimeKind.Local).AddTicks(5507), null });

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ImagePath" },
                values: new object[] { new DateTime(2025, 7, 16, 13, 1, 27, 984, DateTimeKind.Local).AddTicks(5508), null });

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ImagePath" },
                values: new object[] { new DateTime(2025, 7, 16, 13, 1, 27, 984, DateTimeKind.Local).AddTicks(5509), null });

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ImagePath" },
                values: new object[] { new DateTime(2025, 7, 16, 13, 1, 27, 984, DateTimeKind.Local).AddTicks(5511), null });

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "ImagePath" },
                values: new object[] { new DateTime(2025, 7, 16, 13, 1, 27, 984, DateTimeKind.Local).AddTicks(5513), null });

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedDate", "ImagePath" },
                values: new object[] { new DateTime(2025, 7, 16, 13, 1, 27, 984, DateTimeKind.Local).AddTicks(5514), null });

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedDate", "ImagePath" },
                values: new object[] { new DateTime(2025, 7, 16, 13, 1, 27, 984, DateTimeKind.Local).AddTicks(5515), null });

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "ImagePath" },
                values: new object[] { new DateTime(2025, 7, 16, 13, 1, 27, 984, DateTimeKind.Local).AddTicks(5517), null });

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 10, 1, 27, 984, DateTimeKind.Utc).AddTicks(7867));

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 10, 1, 27, 984, DateTimeKind.Utc).AddTicks(7873));

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 10, 1, 27, 984, DateTimeKind.Utc).AddTicks(7875));

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 10, 1, 27, 984, DateTimeKind.Utc).AddTicks(7876));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "DigitalPlatforms");

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 14, 13, 20, 28, 529, DateTimeKind.Local).AddTicks(8840));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 14, 13, 20, 28, 529, DateTimeKind.Local).AddTicks(8852));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 14, 13, 20, 28, 529, DateTimeKind.Local).AddTicks(8853));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 14, 13, 20, 28, 529, DateTimeKind.Local).AddTicks(8855));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 14, 13, 20, 28, 529, DateTimeKind.Local).AddTicks(8856));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 14, 13, 20, 28, 529, DateTimeKind.Local).AddTicks(8857));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 14, 13, 20, 28, 529, DateTimeKind.Local).AddTicks(8858));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 14, 13, 20, 28, 529, DateTimeKind.Local).AddTicks(8860));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 14, 13, 20, 28, 529, DateTimeKind.Local).AddTicks(8861));

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 14, 10, 20, 28, 530, DateTimeKind.Utc).AddTicks(1301));

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 14, 10, 20, 28, 530, DateTimeKind.Utc).AddTicks(1308));

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 14, 10, 20, 28, 530, DateTimeKind.Utc).AddTicks(1309));

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 14, 10, 20, 28, 530, DateTimeKind.Utc).AddTicks(1310));
        }
    }
}
