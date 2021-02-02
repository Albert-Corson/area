using Microsoft.EntityFrameworkCore.Migrations;

namespace Area.API.Migrations
{
    public partial class RmRequiredFromUserWidgetParam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "Required",
                "UserHasWidgetParams");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                "Required",
                "UserHasWidgetParams",
                "boolean",
                nullable: true);
        }
    }
}