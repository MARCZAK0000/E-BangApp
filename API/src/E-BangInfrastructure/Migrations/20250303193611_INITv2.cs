using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_BangInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class INITv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "Account",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "StreetNumber",
                schema: "Account",
                table: "UserAddress",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                schema: "Account",
                table: "UserAddress",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                schema: "Account",
                table: "UserAddress",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                schema: "Account",
                table: "UserAddress",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "StreetName",
                schema: "Account",
                table: "UserAddress",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "Account",
                table: "User",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                schema: "Account",
                table: "User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "Account",
                table: "User",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "SecondName",
                schema: "Account",
                table: "User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                schema: "Account",
                table: "User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                schema: "Security",
                table: "Role",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "RoleDescription",
                schema: "Security",
                table: "Role",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_Country",
                schema: "Account",
                table: "UserAddress",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                schema: "Account",
                table: "User",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_User_Surname",
                schema: "Account",
                table: "User",
                column: "Surname");

            migrationBuilder.CreateIndex(
                name: "IX_Role_RoleName",
                schema: "Security",
                table: "Role",
                column: "RoleName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserAddress_Country",
                schema: "Account",
                table: "UserAddress");

            migrationBuilder.DropIndex(
                name: "IX_User_Email",
                schema: "Account",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_Surname",
                schema: "Account",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Role_RoleName",
                schema: "Security",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "StreetName",
                schema: "Account",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "SecondName",
                schema: "Account",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Surname",
                schema: "Account",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "StreetNumber",
                schema: "Account",
                table: "UserAddress",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                schema: "Account",
                table: "UserAddress",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(7)",
                oldMaxLength: 7);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                schema: "Account",
                table: "UserAddress",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                schema: "Account",
                table: "UserAddress",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "Account",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                schema: "Account",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "Account",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "Account",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                schema: "Security",
                table: "Role",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "RoleDescription",
                schema: "Security",
                table: "Role",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);
        }
    }
}
