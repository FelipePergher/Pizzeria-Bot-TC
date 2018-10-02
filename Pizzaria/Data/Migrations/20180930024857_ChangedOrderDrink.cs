using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pizzaria.Migrations
{
    public partial class ChangedOrderDrink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderDrinks");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "OrderDrinks");

            migrationBuilder.CreateTable(
                name: "OrderDrinkSize",
                columns: table => new
                {
                    OrderDrinkSizeId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DrinkSizeSizeDId = table.Column<int>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    OrderDrinkDrinkId = table.Column<int>(nullable: true),
                    OrderDrinkOrderId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDrinkSize", x => x.OrderDrinkSizeId);
                    table.ForeignKey(
                        name: "FK_OrderDrinkSize_SizesD_DrinkSizeSizeDId",
                        column: x => x.DrinkSizeSizeDId,
                        principalTable: "SizesD",
                        principalColumn: "SizeDId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDrinkSize_OrderDrinks_OrderDrinkOrderId_OrderDrinkDrink~",
                        columns: x => new { x.OrderDrinkOrderId, x.OrderDrinkDrinkId },
                        principalTable: "OrderDrinks",
                        principalColumns: new[] { "OrderId", "DrinkId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDrinkSize_DrinkSizeSizeDId",
                table: "OrderDrinkSize",
                column: "DrinkSizeSizeDId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDrinkSize_OrderDrinkOrderId_OrderDrinkDrinkId",
                table: "OrderDrinkSize",
                columns: new[] { "OrderDrinkOrderId", "OrderDrinkDrinkId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDrinkSize");

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
    }
}
