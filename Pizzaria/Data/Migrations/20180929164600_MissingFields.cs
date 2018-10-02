using Microsoft.EntityFrameworkCore.Migrations;

namespace Pizzaria.Migrations
{
    public partial class MissingFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Delivery",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "OrderPizzas",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "OrderPizzas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "OrderDrinks",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "OrderDrinks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderPizzas");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "OrderPizzas");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderDrinks");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "OrderDrinks");

            migrationBuilder.AlterColumn<double>(
                name: "Delivery",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(bool));
        }
    }
}
