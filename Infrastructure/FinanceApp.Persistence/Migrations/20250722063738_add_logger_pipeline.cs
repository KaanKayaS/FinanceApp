using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class add_logger_pipeline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DurationMs = table.Column<int>(type: "int", nullable: false),
                    Success = table.Column<bool>(type: "bit", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 13, 1, 27, 984, DateTimeKind.Local).AddTicks(5495));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 13, 1, 27, 984, DateTimeKind.Local).AddTicks(5507));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 13, 1, 27, 984, DateTimeKind.Local).AddTicks(5508));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 13, 1, 27, 984, DateTimeKind.Local).AddTicks(5509));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 13, 1, 27, 984, DateTimeKind.Local).AddTicks(5511));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 13, 1, 27, 984, DateTimeKind.Local).AddTicks(5513));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 13, 1, 27, 984, DateTimeKind.Local).AddTicks(5514));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 13, 1, 27, 984, DateTimeKind.Local).AddTicks(5515));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 13, 1, 27, 984, DateTimeKind.Local).AddTicks(5517));

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
    }
}
