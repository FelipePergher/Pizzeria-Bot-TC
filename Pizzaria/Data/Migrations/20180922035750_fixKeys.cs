using Microsoft.EntityFrameworkCore.Migrations;

namespace Pizzaria.Migrations
{
    public partial class fixKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_PizzaSize_PizzaSizeId",
                table: "PizzaSize");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_DrinkSize_DrinkSizeId",
                table: "DrinkSize");

            migrationBuilder.DropColumn(
                name: "PizzaSizeId",
                table: "PizzaSize");

            migrationBuilder.DropColumn(
                name: "DrinkSizeId",
                table: "DrinkSize");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PizzaSizeId",
                table: "PizzaSize",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DrinkSizeId",
                table: "DrinkSize",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PizzaSize_PizzaSizeId",
                table: "PizzaSize",
                column: "PizzaSizeId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_DrinkSize_DrinkSizeId",
                table: "DrinkSize",
                column: "DrinkSizeId");
        }
    }
}
