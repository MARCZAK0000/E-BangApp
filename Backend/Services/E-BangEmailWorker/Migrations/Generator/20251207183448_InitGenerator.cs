using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_BangEmailWorker.Migrations.Generator
{
    /// <inheritdoc />
    public partial class InitGenerator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssemblyEntityTypes",
                columns: table => new
                {
                    AssemblyEntityTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssemblyEntityTypeName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssemblyEntityTypes", x => x.AssemblyEntityTypeId);
                });

            migrationBuilder.CreateTable(
                name: "AssemblyTypes",
                columns: table => new
                {
                    AssemblyTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssemblyName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    AssemblyPath = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssemblyTypes", x => x.AssemblyTypeId);
                });

            migrationBuilder.CreateTable(
                name: "EmailTypes",
                columns: table => new
                {
                    EmailTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmailTypeName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTypes", x => x.EmailTypeId);
                });

            migrationBuilder.CreateTable(
                name: "RenderStrategies",
                columns: table => new
                {
                    RenderStrategyId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssemblyPath = table.Column<string>(type: "TEXT", nullable: false),
                    AssemblyName = table.Column<string>(type: "TEXT", nullable: false),
                    RenderStrategyName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenderStrategies", x => x.RenderStrategyId);
                });

            migrationBuilder.CreateTable(
                name: "AssemblyComponentEntities",
                columns: table => new
                {
                    AssemblyComponentEntityId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssemblyEntityTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssemblyTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ComponentName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ComponentValue = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssemblyComponentEntities", x => x.AssemblyComponentEntityId);
                    table.ForeignKey(
                        name: "FK_AssemblyComponentEntities_AssemblyEntityTypes_AssemblyEntityTypeId",
                        column: x => x.AssemblyEntityTypeId,
                        principalTable: "AssemblyEntityTypes",
                        principalColumn: "AssemblyEntityTypeId");
                    table.ForeignKey(
                        name: "FK_AssemblyComponentEntities_AssemblyTypes_AssemblyTypeId",
                        column: x => x.AssemblyTypeId,
                        principalTable: "AssemblyTypes",
                        principalColumn: "AssemblyTypeId");
                });

            migrationBuilder.CreateTable(
                name: "AssemblyParametersEntities",
                columns: table => new
                {
                    AssemblyParametersEntityId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssemblyEntityTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssemblyTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityParametersName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    EntityParametersValue = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssemblyParametersEntities", x => x.AssemblyParametersEntityId);
                    table.ForeignKey(
                        name: "FK_AssemblyParametersEntities_AssemblyEntityTypes_AssemblyEntityTypeId",
                        column: x => x.AssemblyEntityTypeId,
                        principalTable: "AssemblyEntityTypes",
                        principalColumn: "AssemblyEntityTypeId");
                    table.ForeignKey(
                        name: "FK_AssemblyParametersEntities_AssemblyTypes_AssemblyTypeId",
                        column: x => x.AssemblyTypeId,
                        principalTable: "AssemblyTypes",
                        principalColumn: "AssemblyTypeId");
                });

            migrationBuilder.CreateTable(
                name: "EmailRenders",
                columns: table => new
                {
                    EmailRenderId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmailTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    EmailRenderStrategyId = table.Column<int>(type: "INTEGER", nullable: false),
                    HeaderParametersId = table.Column<int>(type: "INTEGER", nullable: false),
                    BodyParametersId = table.Column<int>(type: "INTEGER", nullable: false),
                    FooterParametersId = table.Column<int>(type: "INTEGER", nullable: false),
                    HeaderComponenetsId = table.Column<int>(type: "INTEGER", nullable: false),
                    BodyComponenetsId = table.Column<int>(type: "INTEGER", nullable: false),
                    FooterComponenetsId = table.Column<int>(type: "INTEGER", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailRenders", x => x.EmailRenderId);
                    table.ForeignKey(
                        name: "FK_EmailRenders_AssemblyComponentEntities_BodyComponenetsId",
                        column: x => x.BodyComponenetsId,
                        principalTable: "AssemblyComponentEntities",
                        principalColumn: "AssemblyComponentEntityId");
                    table.ForeignKey(
                        name: "FK_EmailRenders_AssemblyComponentEntities_FooterComponenetsId",
                        column: x => x.FooterComponenetsId,
                        principalTable: "AssemblyComponentEntities",
                        principalColumn: "AssemblyComponentEntityId");
                    table.ForeignKey(
                        name: "FK_EmailRenders_AssemblyComponentEntities_HeaderComponenetsId",
                        column: x => x.HeaderComponenetsId,
                        principalTable: "AssemblyComponentEntities",
                        principalColumn: "AssemblyComponentEntityId");
                    table.ForeignKey(
                        name: "FK_EmailRenders_AssemblyParametersEntities_BodyParametersId",
                        column: x => x.BodyParametersId,
                        principalTable: "AssemblyParametersEntities",
                        principalColumn: "AssemblyParametersEntityId");
                    table.ForeignKey(
                        name: "FK_EmailRenders_AssemblyParametersEntities_FooterParametersId",
                        column: x => x.FooterParametersId,
                        principalTable: "AssemblyParametersEntities",
                        principalColumn: "AssemblyParametersEntityId");
                    table.ForeignKey(
                        name: "FK_EmailRenders_AssemblyParametersEntities_HeaderParametersId",
                        column: x => x.HeaderParametersId,
                        principalTable: "AssemblyParametersEntities",
                        principalColumn: "AssemblyParametersEntityId");
                    table.ForeignKey(
                        name: "FK_EmailRenders_EmailTypes_EmailTypeId",
                        column: x => x.EmailTypeId,
                        principalTable: "EmailTypes",
                        principalColumn: "EmailTypeId");
                    table.ForeignKey(
                        name: "FK_EmailRenders_RenderStrategies_EmailRenderStrategyId",
                        column: x => x.EmailRenderStrategyId,
                        principalTable: "RenderStrategies",
                        principalColumn: "RenderStrategyId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyComponentEntities_AssemblyEntityTypeId",
                table: "AssemblyComponentEntities",
                column: "AssemblyEntityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyComponentEntities_AssemblyTypeId",
                table: "AssemblyComponentEntities",
                column: "AssemblyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyComponentEntities_ComponentName",
                table: "AssemblyComponentEntities",
                column: "ComponentName");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyEntityTypes_AssemblyEntityTypeName",
                table: "AssemblyEntityTypes",
                column: "AssemblyEntityTypeName");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyParametersEntities_AssemblyEntityTypeId",
                table: "AssemblyParametersEntities",
                column: "AssemblyEntityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyParametersEntities_AssemblyTypeId",
                table: "AssemblyParametersEntities",
                column: "AssemblyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyParametersEntities_EntityParametersName",
                table: "AssemblyParametersEntities",
                column: "EntityParametersName");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyTypes_AssemblyName",
                table: "AssemblyTypes",
                column: "AssemblyName");

            migrationBuilder.CreateIndex(
                name: "IX_EmailRenders_BodyComponenetsId",
                table: "EmailRenders",
                column: "BodyComponenetsId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailRenders_BodyParametersId",
                table: "EmailRenders",
                column: "BodyParametersId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailRenders_EmailRenderStrategyId",
                table: "EmailRenders",
                column: "EmailRenderStrategyId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailRenders_EmailTypeId",
                table: "EmailRenders",
                column: "EmailTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailRenders_FooterComponenetsId",
                table: "EmailRenders",
                column: "FooterComponenetsId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailRenders_FooterParametersId",
                table: "EmailRenders",
                column: "FooterParametersId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailRenders_HeaderComponenetsId",
                table: "EmailRenders",
                column: "HeaderComponenetsId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailRenders_HeaderParametersId",
                table: "EmailRenders",
                column: "HeaderParametersId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTypes_EmailTypeName",
                table: "EmailTypes",
                column: "EmailTypeName");

            migrationBuilder.CreateIndex(
                name: "IX_RenderStrategies_RenderStrategyName",
                table: "RenderStrategies",
                column: "RenderStrategyName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailRenders");

            migrationBuilder.DropTable(
                name: "AssemblyComponentEntities");

            migrationBuilder.DropTable(
                name: "AssemblyParametersEntities");

            migrationBuilder.DropTable(
                name: "EmailTypes");

            migrationBuilder.DropTable(
                name: "RenderStrategies");

            migrationBuilder.DropTable(
                name: "AssemblyEntityTypes");

            migrationBuilder.DropTable(
                name: "AssemblyTypes");
        }
    }
}
