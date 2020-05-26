using Microsoft.EntityFrameworkCore.Migrations;

namespace BT.Data.Migrations
{
    public partial class activename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PName",
                table: "Permissions");

            migrationBuilder.AddColumn<string>(
                name: "ActiveName",
                table: "Permissions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveName",
                table: "Permissions");

            migrationBuilder.AddColumn<string>(
                name: "PName",
                table: "Permissions",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
