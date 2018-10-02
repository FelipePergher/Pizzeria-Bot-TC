using Microsoft.EntityFrameworkCore.Migrations;

namespace Pizzaria.Migrations
{
    public partial class FieldMissing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PizzaSizeName",
                table: "OrderPizzas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DrinkSizeName",
                table: "OrderDrinks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PizzaSizeName",
                table: "OrderPizzas");

            migrationBuilder.DropColumn(
                name: "DrinkSizeName",
                table: "OrderDrinks");
        }
    }
}
