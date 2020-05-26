using Microsoft.EntityFrameworkCore.Migrations;

namespace BT.Data.Migrations
{
    public partial class remove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Orders_Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Orders_Products",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
