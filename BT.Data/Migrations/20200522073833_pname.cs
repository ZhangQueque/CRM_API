using Microsoft.EntityFrameworkCore.Migrations;

namespace BT.Data.Migrations
{
    public partial class pname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PName",
                table: "Permissions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PName",
                table: "Permissions");
        }
    }
}
