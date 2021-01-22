using Microsoft.EntityFrameworkCore.Migrations;

namespace Area.API.Migrations
{
    public partial class AddRequiredToWidgetParam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WidgetHasDefaultParams_Widgets_WidgetModelId",
                table: "WidgetHasDefaultParams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WidgetHasDefaultParams",
                table: "WidgetHasDefaultParams");

            migrationBuilder.RenameTable(
                name: "WidgetHasDefaultParams",
                newName: "WidgetHasParams");

            migrationBuilder.AddColumn<bool>(
                name: "Required",
                table: "UserHasWidgetParams",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Required",
                table: "WidgetHasParams",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WidgetHasParams",
                table: "WidgetHasParams",
                columns: new[] { "WidgetModelId", "Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_WidgetHasParams_Widgets_WidgetModelId",
                table: "WidgetHasParams",
                column: "WidgetModelId",
                principalTable: "Widgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WidgetHasParams_Widgets_WidgetModelId",
                table: "WidgetHasParams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WidgetHasParams",
                table: "WidgetHasParams");

            migrationBuilder.DropColumn(
                name: "Required",
                table: "UserHasWidgetParams");

            migrationBuilder.DropColumn(
                name: "Required",
                table: "WidgetHasParams");

            migrationBuilder.RenameTable(
                name: "WidgetHasParams",
                newName: "WidgetHasDefaultParams");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WidgetHasDefaultParams",
                table: "WidgetHasDefaultParams",
                columns: new[] { "WidgetModelId", "Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_WidgetHasDefaultParams_Widgets_WidgetModelId",
                table: "WidgetHasDefaultParams",
                column: "WidgetModelId",
                principalTable: "Widgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
