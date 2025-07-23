using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class add_column_instructions_groupId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "Instructions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 14, 34, 30, 932, DateTimeKind.Local).AddTicks(4263));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 14, 34, 30, 932, DateTimeKind.Local).AddTicks(4277));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 14, 34, 30, 932, DateTimeKind.Local).AddTicks(4278));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 14, 34, 30, 932, DateTimeKind.Local).AddTicks(4279));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 14, 34, 30, 932, DateTimeKind.Local).AddTicks(4281));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 14, 34, 30, 932, DateTimeKind.Local).AddTicks(4282));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 14, 34, 30, 932, DateTimeKind.Local).AddTicks(4283));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 14, 34, 30, 932, DateTimeKind.Local).AddTicks(4284));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 14, 34, 30, 932, DateTimeKind.Local).AddTicks(4328));

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 11, 34, 30, 932, DateTimeKind.Utc).AddTicks(6567));

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 11, 34, 30, 932, DateTimeKind.Utc).AddTicks(6574));

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 11, 34, 30, 932, DateTimeKind.Utc).AddTicks(6575));

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 11, 34, 30, 932, DateTimeKind.Utc).AddTicks(6576));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Instructions");

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 12, 37, 38, 2, DateTimeKind.Local).AddTicks(7717));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 12, 37, 38, 2, DateTimeKind.Local).AddTicks(7734));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 12, 37, 38, 2, DateTimeKind.Local).AddTicks(7736));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 12, 37, 38, 2, DateTimeKind.Local).AddTicks(7737));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 12, 37, 38, 2, DateTimeKind.Local).AddTicks(7739));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 12, 37, 38, 2, DateTimeKind.Local).AddTicks(7740));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 12, 37, 38, 2, DateTimeKind.Local).AddTicks(7742));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 12, 37, 38, 2, DateTimeKind.Local).AddTicks(7743));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 12, 37, 38, 2, DateTimeKind.Local).AddTicks(7744));

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 9, 37, 38, 3, DateTimeKind.Utc).AddTicks(167));

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 9, 37, 38, 3, DateTimeKind.Utc).AddTicks(184));

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 9, 37, 38, 3, DateTimeKind.Utc).AddTicks(185));

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 22, 9, 37, 38, 3, DateTimeKind.Utc).AddTicks(186));
        }
    }
}
