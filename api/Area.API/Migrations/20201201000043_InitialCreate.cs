using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Area.API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Services",
                table => new {
                    Id = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>("text", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Services", x => x.Id); });

            migrationBuilder.CreateTable(
                "Users",
                table => new {
                    Id = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>("text", nullable: true),
                    Email = table.Column<string>("text", nullable: true),
                    Password = table.Column<string>("text", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Users", x => x.Id); });

            migrationBuilder.CreateTable(
                "Widgets",
                table => new {
                    Id = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>("text", nullable: true),
                    Description = table.Column<string>("text", nullable: true),
                    RequiresAuth = table.Column<bool>("boolean", nullable: true),
                    ServiceId = table.Column<int>("integer", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Widgets", x => x.Id);
                    table.ForeignKey(
                        "FK_Widgets_Services_ServiceId",
                        x => x.ServiceId,
                        "Services",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "UserHasServiceTokens",
                table => new {
                    UserModelId = table.Column<int>("integer", nullable: false),
                    Id = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Scheme = table.Column<string>("text", nullable: true),
                    AccessToken = table.Column<string>("text", nullable: true),
                    RefreshToken = table.Column<string>("text", nullable: true),
                    ServiceId = table.Column<int>("integer", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_UserHasServiceTokens", x => new {x.UserModelId, x.Id});
                    table.ForeignKey(
                        "FK_UserHasServiceTokens_Services_ServiceId",
                        x => x.ServiceId,
                        "Services",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_UserHasServiceTokens_Users_UserModelId",
                        x => x.UserModelId,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "UserHasWidgetParams",
                table => new {
                    UserModelId = table.Column<int>("integer", nullable: false),
                    Id = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WidgetId = table.Column<int>("integer", nullable: true),
                    Name = table.Column<string>("text", nullable: true),
                    Type = table.Column<string>("text", nullable: true),
                    Value = table.Column<string>("text", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_UserHasWidgetParams", x => new {x.UserModelId, x.Id});
                    table.ForeignKey(
                        "FK_UserHasWidgetParams_Users_UserModelId",
                        x => x.UserModelId,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_UserHasWidgetParams_Widgets_WidgetId",
                        x => x.WidgetId,
                        "Widgets",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "UsersToWidgets",
                table => new {
                    UserId = table.Column<int>("integer", nullable: false),
                    WidgetId = table.Column<int>("integer", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_UsersToWidgets", x => new {x.UserId, x.WidgetId});
                    table.ForeignKey(
                        "FK_UsersToWidgets_Users_UserId",
                        x => x.UserId,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_UsersToWidgets_Widgets_WidgetId",
                        x => x.WidgetId,
                        "Widgets",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "WidgetHasDefaultParams",
                table => new {
                    WidgetModelId = table.Column<int>("integer", nullable: false),
                    Id = table.Column<int>("integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>("text", nullable: true),
                    Type = table.Column<string>("text", nullable: true),
                    Value = table.Column<string>("text", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_WidgetHasDefaultParams", x => new {x.WidgetModelId, x.Id});
                    table.ForeignKey(
                        "FK_WidgetHasDefaultParams_Widgets_WidgetModelId",
                        x => x.WidgetModelId,
                        "Widgets",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_UserHasServiceTokens_ServiceId",
                "UserHasServiceTokens",
                "ServiceId");

            migrationBuilder.CreateIndex(
                "IX_UserHasWidgetParams_WidgetId",
                "UserHasWidgetParams",
                "WidgetId");

            migrationBuilder.CreateIndex(
                "IX_UsersToWidgets_WidgetId",
                "UsersToWidgets",
                "WidgetId");

            migrationBuilder.CreateIndex(
                "IX_Widgets_ServiceId",
                "Widgets",
                "ServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "UserHasServiceTokens");

            migrationBuilder.DropTable(
                "UserHasWidgetParams");

            migrationBuilder.DropTable(
                "UsersToWidgets");

            migrationBuilder.DropTable(
                "WidgetHasDefaultParams");

            migrationBuilder.DropTable(
                "Users");

            migrationBuilder.DropTable(
                "Widgets");

            migrationBuilder.DropTable(
                "Services");
        }
    }
}