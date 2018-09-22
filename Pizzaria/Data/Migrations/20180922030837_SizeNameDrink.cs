using Microsoft.EntityFrameworkCore.Migrations;

namespace Pizzaria.Migrations
{
    public partial class SizeNameDrink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SizeName",
                table: "SizesD",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SizeName",
                table: "SizesD");
        }
    }
}
