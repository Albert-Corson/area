using Microsoft.EntityFrameworkCore.Migrations;

namespace Area.API.Migrations
{
    public partial class RmRequiredFromUserWidgetParam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Required",
                table: "UserHasWidgetParams");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Required",
                table: "UserHasWidgetParams",
                type: "boolean",
                nullable: true);
        }
    }
}
