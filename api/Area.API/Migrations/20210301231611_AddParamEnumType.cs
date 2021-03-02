using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Area.API.Migrations
{
    public partial class AddParamEnumType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Required",
                table: "Params");

            migrationBuilder.CreateTable(
                name: "Enums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enums", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnumHasValues",
                columns: table => new
                {
                    EnumModelId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnumHasValues", x => new { x.EnumModelId, x.Id });
                    table.ForeignKey(
                        name: "FK_EnumHasValues_Enums_EnumModelId",
                        column: x => x.EnumModelId,
                        principalTable: "Enums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParamsToEnums",
                columns: table => new
                {
                    ParamId = table.Column<int>(type: "integer", nullable: false),
                    EnumId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParamsToEnums", x => new { x.ParamId, x.EnumId });
                    table.ForeignKey(
                        name: "FK_ParamsToEnums_Enums_EnumId",
                        column: x => x.EnumId,
                        principalTable: "Enums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParamsToEnums_Params_ParamId",
                        column: x => x.ParamId,
                        principalTable: "Params",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParamsToEnums_EnumId",
                table: "ParamsToEnums",
                column: "EnumId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnumHasValues");

            migrationBuilder.DropTable(
                name: "ParamsToEnums");

            migrationBuilder.DropTable(
                name: "Enums");

            migrationBuilder.AddColumn<bool>(
                name: "Required",
                table: "Params",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
