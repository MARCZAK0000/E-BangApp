using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_BangInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class actionChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionInRole",
                schema: "Security");

            migrationBuilder.AddColumn<int>(
                name: "ActionLevel",
                schema: "Shop",
                table: "ShopStaff",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActionLevel",
                schema: "Security",
                table: "Action",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionLevel",
                schema: "Shop",
                table: "ShopStaff");

            migrationBuilder.DropColumn(
                name: "ActionLevel",
                schema: "Security",
                table: "Action");

            migrationBuilder.CreateTable(
                name: "ActionInRole",
                schema: "Security",
                columns: table => new
                {
                    ActionInRoleID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActionID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionInRole", x => x.ActionInRoleID);
                    table.ForeignKey(
                        name: "FK_ActionInRole_Action_ActionID",
                        column: x => x.ActionID,
                        principalSchema: "Security",
                        principalTable: "Action",
                        principalColumn: "ActionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionInRole_Role_RoleID",
                        column: x => x.RoleID,
                        principalSchema: "Security",
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionInRole_ActionID",
                schema: "Security",
                table: "ActionInRole",
                column: "ActionID");

            migrationBuilder.CreateIndex(
                name: "IX_ActionInRole_RoleID",
                schema: "Security",
                table: "ActionInRole",
                column: "RoleID");
        }
    }
}
