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
            migrationBuilder.CreateTable(
                name: "Notifcations",
                columns: table => new
                {
                    NotificationId = table.Column<string>(type: "text", nullable: false),
                    ReciverId = table.Column<string>(type: "text", nullable: false),
                    ReciverName = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    SenderId = table.Column<string>(type: "text", nullable: false),
                    SenderName = table.Column<string>(type: "text", nullable: false),
                    IsReaded = table.Column<bool>(type: "boolean", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifcations", x => x.NotificationId);
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
        }
    }
}
