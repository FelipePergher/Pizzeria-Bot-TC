using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pizzaria.Migrations
{
    public partial class ChangedOrderPizza : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderPizzas");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "OrderPizzas");

            migrationBuilder.CreateTable(
                name: "OrderPizzaSize",
                columns: table => new
                {
                    OrderPizzaSizeId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PizzaSizeSizePId = table.Column<int>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    OrderPizzaOrderId = table.Column<int>(nullable: true),
                    OrderPizzaPizzaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPizzaSize", x => x.OrderPizzaSizeId);
                    table.ForeignKey(
                        name: "FK_OrderPizzaSize_SizesP_PizzaSizeSizePId",
                        column: x => x.PizzaSizeSizePId,
                        principalTable: "SizesP",
                        principalColumn: "SizePId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderPizzaSize_OrderPizzas_OrderPizzaOrderId_OrderPizzaPizza~",
                        columns: x => new { x.OrderPizzaOrderId, x.OrderPizzaPizzaId },
                        principalTable: "OrderPizzas",
                        principalColumns: new[] { "OrderId", "PizzaId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderPizzaSize_PizzaSizeSizePId",
                table: "OrderPizzaSize",
                column: "PizzaSizeSizePId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPizzaSize_OrderPizzaOrderId_OrderPizzaPizzaId",
                table: "OrderPizzaSize",
                columns: new[] { "OrderPizzaOrderId", "OrderPizzaPizzaId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderPizzaSize");

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
        }
    }
}
