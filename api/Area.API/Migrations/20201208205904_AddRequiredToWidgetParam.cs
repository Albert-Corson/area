using Microsoft.EntityFrameworkCore.Migrations;

namespace Area.API.Migrations
{
    public partial class AddRequiredToWidgetParam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_WidgetHasDefaultParams_Widgets_WidgetModelId",
                "WidgetHasDefaultParams");

            migrationBuilder.DropPrimaryKey(
                "PK_WidgetHasDefaultParams",
                "WidgetHasDefaultParams");

            migrationBuilder.RenameTable(
                "WidgetHasDefaultParams",
                newName: "WidgetHasParams");

            migrationBuilder.AddColumn<bool>(
                "Required",
                "UserHasWidgetParams",
                "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                "Required",
                "WidgetHasParams",
                "boolean",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                "PK_WidgetHasParams",
                "WidgetHasParams",
                new[] {"WidgetModelId", "Id"});

            migrationBuilder.AddForeignKey(
                "FK_WidgetHasParams_Widgets_WidgetModelId",
                "WidgetHasParams",
                "WidgetModelId",
                "Widgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_WidgetHasParams_Widgets_WidgetModelId",
                "WidgetHasParams");

            migrationBuilder.DropPrimaryKey(
                "PK_WidgetHasParams",
                "WidgetHasParams");

            migrationBuilder.DropColumn(
                "Required",
                "UserHasWidgetParams");

            migrationBuilder.DropColumn(
                "Required",
                "WidgetHasParams");

            migrationBuilder.RenameTable(
                "WidgetHasParams",
                newName: "WidgetHasDefaultParams");

            migrationBuilder.AddPrimaryKey(
                "PK_WidgetHasDefaultParams",
                "WidgetHasDefaultParams",
                new[] {"WidgetModelId", "Id"});

            migrationBuilder.AddForeignKey(
                "FK_WidgetHasDefaultParams_Widgets_WidgetModelId",
                "WidgetHasDefaultParams",
                "WidgetModelId",
                "Widgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}