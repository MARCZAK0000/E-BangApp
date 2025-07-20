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
                name: "Product");

            migrationBuilder.EnsureSchema(
                name: "Shop");

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
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TwoFactoryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RoleDescription = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    RoleLevel = table.Column<int>(type: "int", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "ShopType",
                schema: "Shop",
                columns: table => new
                {
                    ShopTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopType", x => x.ShopTypeId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionInRole_Role_RoleID",
                        column: x => x.RoleID,
                        principalSchema: "Security",
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Account",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SecondName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RolesRoleID = table.Column<string>(type: "nvarchar(450)", nullable: true)
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
                        name: "FK_User_Role_RolesRoleID",
                        column: x => x.RolesRoleID,
                        principalSchema: "Security",
                        principalTable: "Role",
                        principalColumn: "RoleID");
                });

            migrationBuilder.CreateTable(
                name: "Shop",
                schema: "Shop",
                columns: table => new
                {
                    ShopId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShopName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShopDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ShopTypeId = table.Column<int>(type: "int", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shop", x => x.ShopId);
                    table.ForeignKey(
                        name: "FK_Shop_ShopType_ShopTypeId",
                        column: x => x.ShopTypeId,
                        principalSchema: "Shop",
                        principalTable: "ShopType",
                        principalColumn: "ShopTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAddress",
                schema: "Account",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StreetNumber = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    StreetName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
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

            migrationBuilder.CreateTable(
                name: "UsersInRole",
                schema: "Security",
                columns: table => new
                {
                    UserInRoleID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersInRole", x => x.UserInRoleID);
                    table.ForeignKey(
                        name: "FK_UsersInRole_Role_RoleID",
                        column: x => x.RoleID,
                        principalSchema: "Security",
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersInRole_User_UserID",
                        column: x => x.UserID,
                        principalSchema: "Account",
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "Product",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProductDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ShopID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Product_Shop_ShopID",
                        column: x => x.ShopID,
                        principalSchema: "Shop",
                        principalTable: "Shop",
                        principalColumn: "ShopId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopStaff",
                schema: "Shop",
                columns: table => new
                {
                    ShopStaffId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShopId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopStaff", x => x.ShopStaffId);
                    table.ForeignKey(
                        name: "FK_ShopStaff_Shop_ShopId",
                        column: x => x.ShopId,
                        principalSchema: "Shop",
                        principalTable: "Shop",
                        principalColumn: "ShopId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopStaff_User_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Account",
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductInformations",
                schema: "Product",
                columns: table => new
                {
                    ProductID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InformationsType = table.Column<int>(type: "int", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInformations", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_ProductInformations_Product_ProductID",
                        column: x => x.ProductID,
                        principalSchema: "Product",
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPrice",
                schema: "Product",
                columns: table => new
                {
                    ProductID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductPriceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(2,2)", precision: 2, nullable: false),
                    ProductCount = table.Column<int>(type: "int", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPrice", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_ProductPrice_Product_ProductID",
                        column: x => x.ProductID,
                        principalSchema: "Product",
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopBranchesInformations",
                schema: "Shop",
                columns: table => new
                {
                    ShopBranchId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShopID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShopCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ShopCountry = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ShopPostalCode = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    ShopStreetName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsMainShop = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopBranchesInformations", x => x.ShopBranchId);
                    table.ForeignKey(
                        name: "FK_ShopBranchesInformations_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Product",
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopBranchesInformations_Shop_ShopID",
                        column: x => x.ShopID,
                        principalSchema: "Shop",
                        principalTable: "Shop",
                        principalColumn: "ShopId",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductName",
                schema: "Product",
                table: "Product",
                column: "ProductName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ShopID",
                schema: "Product",
                table: "Product",
                column: "ShopID");

            migrationBuilder.CreateIndex(
                name: "IX_Role_RoleName",
                schema: "Security",
                table: "Role",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shop_ShopName",
                schema: "Shop",
                table: "Shop",
                column: "ShopName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shop_ShopTypeId",
                schema: "Shop",
                table: "Shop",
                column: "ShopTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopBranchesInformations_ProductId",
                schema: "Shop",
                table: "ShopBranchesInformations",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopBranchesInformations_ShopID",
                schema: "Shop",
                table: "ShopBranchesInformations",
                column: "ShopID");

            migrationBuilder.CreateIndex(
                name: "IX_ShopStaff_AccountId",
                schema: "Shop",
                table: "ShopStaff",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopStaff_ShopId",
                schema: "Shop",
                table: "ShopStaff",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopType_ShopTypeName",
                schema: "Shop",
                table: "ShopType",
                column: "ShopTypeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                schema: "Account",
                table: "User",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_User_RolesRoleID",
                schema: "Account",
                table: "User",
                column: "RolesRoleID");

            migrationBuilder.CreateIndex(
                name: "IX_User_Surname",
                schema: "Account",
                table: "User",
                column: "Surname");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_Country",
                schema: "Account",
                table: "UserAddress",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_UsersInRole_RoleID",
                schema: "Security",
                table: "UsersInRole",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_UsersInRole_UserID_RoleID",
                schema: "Security",
                table: "UsersInRole",
                columns: new[] { "UserID", "RoleID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionInRole",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ProductInformations",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "ProductPrice",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "ShopBranchesInformations",
                schema: "Shop");

            migrationBuilder.DropTable(
                name: "ShopStaff",
                schema: "Shop");

            migrationBuilder.DropTable(
                name: "UserAddress",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "UsersInRole",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Action",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Shop",
                schema: "Shop");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "ShopType",
                schema: "Shop");
        }
    }
}
