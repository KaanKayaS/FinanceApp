using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class add_balancememory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BalanceMemories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditCardId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalanceMemories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BalanceMemories_CreditCards_CreditCardId",
                        column: x => x.CreditCardId,
                        principalTable: "CreditCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_BalanceMemories_CreditCardId",
                table: "BalanceMemories",
                column: "CreditCardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BalanceMemories");

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 11, 13, 54, 9, 978, DateTimeKind.Local).AddTicks(5437));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 11, 13, 54, 9, 978, DateTimeKind.Local).AddTicks(5449));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 11, 13, 54, 9, 978, DateTimeKind.Local).AddTicks(5451));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 11, 13, 54, 9, 978, DateTimeKind.Local).AddTicks(5452));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 11, 13, 54, 9, 978, DateTimeKind.Local).AddTicks(5454));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 11, 13, 54, 9, 978, DateTimeKind.Local).AddTicks(5455));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 11, 13, 54, 9, 978, DateTimeKind.Local).AddTicks(5456));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 11, 13, 54, 9, 978, DateTimeKind.Local).AddTicks(5458));

            migrationBuilder.UpdateData(
                table: "DigitalPlatforms",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 11, 13, 54, 9, 978, DateTimeKind.Local).AddTicks(5459));

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 11, 10, 54, 9, 978, DateTimeKind.Utc).AddTicks(7710));

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 11, 10, 54, 9, 978, DateTimeKind.Utc).AddTicks(7716));

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 11, 10, 54, 9, 978, DateTimeKind.Utc).AddTicks(7717));

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 11, 10, 54, 9, 978, DateTimeKind.Utc).AddTicks(7718));
        }
    }
}
