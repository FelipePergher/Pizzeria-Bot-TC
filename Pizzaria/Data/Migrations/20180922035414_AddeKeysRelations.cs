using Microsoft.EntityFrameworkCore.Migrations;

namespace Pizzaria.Migrations
{
    public partial class AddeKeysRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_PizzaSize_PizzaSizeId",
                table: "PizzaSize",
                column: "PizzaSizeId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_DrinkSize_DrinkSizeId",
                table: "DrinkSize",
                column: "DrinkSizeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_PizzaSize_PizzaSizeId",
                table: "PizzaSize");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_DrinkSize_DrinkSizeId",
                table: "DrinkSize");
        }
    }
}
