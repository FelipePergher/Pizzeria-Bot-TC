using Microsoft.EntityFrameworkCore.Migrations;

namespace Pizzaria.Migrations
{
    public partial class ImageDrinkAndPizza : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PizzaIngredient",
                table: "PizzaIngredient");

            migrationBuilder.DropIndex(
                name: "IX_PizzaIngredient_PizzaId",
                table: "PizzaIngredient");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Pizzas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PizzaType",
                table: "Pizzas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Drinks",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PizzaIngredient",
                table: "PizzaIngredient",
                columns: new[] { "PizzaId", "IngredientId" });

            migrationBuilder.CreateIndex(
                name: "IX_PizzaIngredient_IngredientId",
                table: "PizzaIngredient",
                column: "IngredientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PizzaIngredient",
                table: "PizzaIngredient");

            migrationBuilder.DropIndex(
                name: "IX_PizzaIngredient_IngredientId",
                table: "PizzaIngredient");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Pizzas");

            migrationBuilder.DropColumn(
                name: "PizzaType",
                table: "Pizzas");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Drinks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PizzaIngredient",
                table: "PizzaIngredient",
                columns: new[] { "IngredientId", "PizzaId" });

            migrationBuilder.CreateIndex(
                name: "IX_PizzaIngredient_PizzaId",
                table: "PizzaIngredient",
                column: "PizzaId");
        }
    }
}
