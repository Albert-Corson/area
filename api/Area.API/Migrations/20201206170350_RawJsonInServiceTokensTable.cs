using Microsoft.EntityFrameworkCore.Migrations;

namespace Area.API.Migrations
{
    public partial class RawJsonInServiceTokensTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "UserHasServiceTokens");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "UserHasServiceTokens");

            migrationBuilder.RenameColumn(
                name: "Scheme",
                table: "UserHasServiceTokens",
                newName: "Json");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Json",
                table: "UserHasServiceTokens",
                newName: "Scheme");

            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "UserHasServiceTokens",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "UserHasServiceTokens",
                type: "text",
                nullable: true);
        }
    }
}
