using Microsoft.EntityFrameworkCore.Migrations;

namespace Area.API.Migrations
{
    public partial class DeviceIdAsUint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHasDevice_AspNetUsers_UserModelId",
                table: "UserHasDevice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHasDevice",
                table: "UserHasDevice");

            migrationBuilder.RenameTable(
                name: "UserHasDevice",
                newName: "UserHasDevices");

            migrationBuilder.RenameIndex(
                name: "IX_UserHasDevice_UserModelId",
                table: "UserHasDevices",
                newName: "IX_UserHasDevices_UserModelId");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "UserHasDevices",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHasDevices",
                table: "UserHasDevices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHasDevices_AspNetUsers_UserModelId",
                table: "UserHasDevices",
                column: "UserModelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHasDevices_AspNetUsers_UserModelId",
                table: "UserHasDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHasDevices",
                table: "UserHasDevices");

            migrationBuilder.RenameTable(
                name: "UserHasDevices",
                newName: "UserHasDevice");

            migrationBuilder.RenameIndex(
                name: "IX_UserHasDevices_UserModelId",
                table: "UserHasDevice",
                newName: "IX_UserHasDevice_UserModelId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserHasDevice",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHasDevice",
                table: "UserHasDevice",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHasDevice_AspNetUsers_UserModelId",
                table: "UserHasDevice",
                column: "UserModelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
