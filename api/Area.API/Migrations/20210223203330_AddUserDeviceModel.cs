using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Area.API.Migrations
{
    public partial class AddUserDeviceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserHasDevice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    FirstUsed = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUsed = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    Device = table.Column<int>(type: "integer", nullable: false),
                    Browser = table.Column<int>(type: "integer", nullable: false),
                    BrowserVersion = table.Column<string>(type: "text", nullable: false),
                    Os = table.Column<int>(type: "integer", nullable: false),
                    OsVersion = table.Column<string>(type: "text", nullable: false),
                    Architecture = table.Column<int>(type: "integer", nullable: false),
                    UserModelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHasDevice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserHasDevice_AspNetUsers_UserModelId",
                        column: x => x.UserModelId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserHasDevice_UserModelId",
                table: "UserHasDevice",
                column: "UserModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserHasDevice");
        }
    }
}
