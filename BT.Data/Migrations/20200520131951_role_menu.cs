using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BT.Data.Migrations
{
    public partial class role_menu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDel = table.Column<int>(nullable: false),
                    CreateID = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Leave = table.Column<int>(nullable: false),
                    Sort = table.Column<int>(nullable: false),
                    ShortDescribe = table.Column<string>(nullable: true),
                    PathCode = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    PID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Roles_Permissions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDel = table.Column<int>(nullable: false),
                    CreateID = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    RoleID = table.Column<int>(nullable: false),
                    RoleName = table.Column<string>(nullable: true),
                    PermissionID = table.Column<int>(nullable: false),
                    PermissionName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles_Permissions", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles_Permissions");
        }
    }
}
