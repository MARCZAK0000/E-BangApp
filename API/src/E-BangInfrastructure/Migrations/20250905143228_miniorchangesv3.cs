using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_BangInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class miniorchangesv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpireDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TwoFactoryCodeExpireDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshTokenExpireDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactoryCodeExpireDate",
                table: "AspNetUsers");
        }
    }
}
