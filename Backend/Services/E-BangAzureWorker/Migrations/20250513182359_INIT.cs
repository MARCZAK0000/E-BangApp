using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_BangAzureWorker.Migrations
{
    /// <inheritdoc />
    public partial class INIT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContainerRoot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RootPath = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContainerRoot", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Containers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RootFilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BlobRootPathID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Containers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Containers_ContainerRoot_BlobRootPathID",
                        column: x => x.BlobRootPathID,
                        principalTable: "ContainerRoot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    BlobItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlobItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlobItemType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContainerID = table.Column<int>(type: "int", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.BlobItemId);
                    table.ForeignKey(
                        name: "FK_Items_Containers_ContainerID",
                        column: x => x.ContainerID,
                        principalTable: "Containers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContainerRoot_RootPath",
                table: "ContainerRoot",
                column: "RootPath",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Containers_BlobRootPathID",
                table: "Containers",
                column: "BlobRootPathID");

            migrationBuilder.CreateIndex(
                name: "IX_Containers_Name",
                table: "Containers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_BlobItemId",
                table: "Items",
                column: "BlobItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_ContainerID",
                table: "Items",
                column: "ContainerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Containers");

            migrationBuilder.DropTable(
                name: "ContainerRoot");
        }
    }
}
