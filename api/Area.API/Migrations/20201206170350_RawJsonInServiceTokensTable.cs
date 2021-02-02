using Microsoft.EntityFrameworkCore.Migrations;

namespace Area.API.Migrations
{
    public partial class RawJsonInServiceTokensTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "AccessToken",
                "UserHasServiceTokens");

            migrationBuilder.DropColumn(
                "RefreshToken",
                "UserHasServiceTokens");

            migrationBuilder.RenameColumn(
                "Scheme",
                "UserHasServiceTokens",
                "Json");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                "Json",
                "UserHasServiceTokens",
                "Scheme");

            migrationBuilder.AddColumn<string>(
                "AccessToken",
                "UserHasServiceTokens",
                "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "RefreshToken",
                "UserHasServiceTokens",
                "text",
                nullable: true);
        }
    }
}