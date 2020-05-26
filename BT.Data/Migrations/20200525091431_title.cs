using Microsoft.EntityFrameworkCore.Migrations;

namespace BT.Data.Migrations
{
    public partial class title : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Titile",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Titile",
                table: "Products",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
