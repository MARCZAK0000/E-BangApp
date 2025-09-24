using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBangNotificationService.Migrations
{
    /// <inheritdoc />
    public partial class INIT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Account");

            migrationBuilder.CreateTable(
                name: "Notifcations",
                columns: table => new
                {
                    NotificationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReciverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReciverName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SenderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsReaded = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifcations", x => x.NotificationId);
                });

            migrationBuilder.CreateTable(
                name: "NotificationSettings",
                schema: "Account",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsEmailNotificationEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsPushNotificationEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsSmsNotificationEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationSettings", x => x.AccountId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifcations_ReciverId",
                table: "Notifcations",
                column: "ReciverId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifcations_SenderId",
                table: "Notifcations",
                column: "SenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifcations");

            migrationBuilder.DropTable(
                name: "NotificationSettings",
                schema: "Account");
        }
    }
}
