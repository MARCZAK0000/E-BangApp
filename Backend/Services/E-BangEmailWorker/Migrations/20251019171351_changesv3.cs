using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_BangEmailWorker.Migrations
{
    /// <inheritdoc />
    public partial class changesv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Current");

            migrationBuilder.EnsureSchema(
                name: "History");

            migrationBuilder.EnsureSchema(
                name: "Settings");

            migrationBuilder.EnsureSchema(
                name: "Command");

            migrationBuilder.CreateTable(
                name: "Email",
                schema: "Current",
                columns: table => new
                {
                    EmailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSend = table.Column<bool>(type: "bit", nullable: false),
                    SendTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email", x => x.EmailId);
                });

            migrationBuilder.CreateTable(
                name: "Emails",
                schema: "History",
                columns: table => new
                {
                    EmailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Recipient = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    HistoryProcessedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Emails__7ED91AEF1294F685", x => x.EmailID);
                });

            migrationBuilder.CreateTable(
                name: "EmailSettings",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmptHost = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureLog",
                schema: "Command",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcedureName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ExecutionTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Procedur__3214EC07DDAC8AC2", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_SentDate",
                schema: "History",
                table: "Emails",
                column: "SentDate");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureLog_ProcedureName",
                schema: "Command",
                table: "ProcedureLog",
                column: "ProcedureName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Email",
                schema: "Current");

            migrationBuilder.DropTable(
                name: "Emails",
                schema: "History");

            migrationBuilder.DropTable(
                name: "EmailSettings",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "ProcedureLog",
                schema: "Command");
        }
    }
}
