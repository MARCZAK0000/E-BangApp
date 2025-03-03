using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_BangInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class INIT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Security");

            migrationBuilder.EnsureSchema(
                name: "Account");

            migrationBuilder.CreateTable(
                name: "Action",
                schema: "Security",
                columns: table => new
                {
                    ActionID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Action", x => x.ActionID);
                });

          
            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "Security",
                columns: table => new
                {
                    RoleID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleID);
                });
                              
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionInRole_Role_RoleID",
                        column: x => x.RoleID,
                        principalSchema: "Security",
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Account",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_User_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Role_UserID",
                        column: x => x.UserID,
                        principalSchema: "Security",
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAddress",
                schema: "Account",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddress", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_UserAddress_User_UserID",
                        column: x => x.UserID,
                        principalSchema: "Account",
                        principalTable: "User",
                        principalColumn: "UserID",
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

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionInRole",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "UserAddress",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Action",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "Security");
        }
    }
}
