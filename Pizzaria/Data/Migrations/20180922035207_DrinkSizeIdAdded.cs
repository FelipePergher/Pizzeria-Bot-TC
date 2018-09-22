using Microsoft.EntityFrameworkCore.Migrations;

namespace Pizzaria.Migrations
{
    public partial class DrinkSizeIdAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DrinkSizeId",
                table: "DrinkSize",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrinkSizeId",
                table: "DrinkSize");
        }
    }
}
