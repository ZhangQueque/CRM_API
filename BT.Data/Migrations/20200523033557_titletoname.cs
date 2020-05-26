using Microsoft.EntityFrameworkCore.Migrations;

namespace BT.Data.Migrations
{
    public partial class titletoname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerSegmentationTitle",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "CustomerSegmentationName",
                table: "Customers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerSegmentationName",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "CustomerSegmentationTitle",
                table: "Customers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
