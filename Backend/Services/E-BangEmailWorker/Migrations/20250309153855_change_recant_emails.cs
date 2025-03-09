using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_BangEmailWorker.Migrations
{
    /// <inheritdoc />
    public partial class change_recant_emails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSend",
                schema: "Recent",
                table: "Email",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSend",
                schema: "Recent",
                table: "Email");
        }
    }
}
