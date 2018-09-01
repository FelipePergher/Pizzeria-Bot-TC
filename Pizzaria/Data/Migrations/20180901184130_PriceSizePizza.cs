using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pizzaria.Migrations
{
    public partial class PriceSizePizza : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrinkSize_Sizes_SizeId",
                table: "DrinkSize");

            migrationBuilder.DropTable(
                name: "Sizes");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Pizzas");

            migrationBuilder.RenameColumn(
                name: "SizeId",
                table: "DrinkSize",
                newName: "SizeDId");

            migrationBuilder.RenameIndex(
                name: "IX_DrinkSize_SizeId",
                table: "DrinkSize",
                newName: "IX_DrinkSize_SizeDId");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "DrinkSize",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "SizesD",
                columns: table => new
                {
                    SizeDId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Quantity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SizesD", x => x.SizeDId);
                });

            migrationBuilder.CreateTable(
                name: "SizesP",
                columns: table => new
                {
                    SizePId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Size = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SizesP", x => x.SizePId);
                });

            migrationBuilder.CreateTable(
                name: "PizzaSize",
                columns: table => new
                {
                    PizzaSizeId = table.Column<int>(nullable: false),
                    SizePId = table.Column<int>(nullable: false),
                    PizzaId = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PizzaSize", x => new { x.PizzaId, x.SizePId });
                    table.ForeignKey(
                        name: "FK_PizzaSize_Pizzas_PizzaId",
                        column: x => x.PizzaId,
                        principalTable: "Pizzas",
                        principalColumn: "PizzaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PizzaSize_SizesP_SizePId",
                        column: x => x.SizePId,
                        principalTable: "SizesP",
                        principalColumn: "SizePId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PizzaSize_SizePId",
                table: "PizzaSize",
                column: "SizePId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkSize_SizesD_SizeDId",
                table: "DrinkSize",
                column: "SizeDId",
                principalTable: "SizesD",
                principalColumn: "SizeDId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrinkSize_SizesD_SizeDId",
                table: "DrinkSize");

            migrationBuilder.DropTable(
                name: "PizzaSize");

            migrationBuilder.DropTable(
                name: "SizesD");

            migrationBuilder.DropTable(
                name: "SizesP");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "DrinkSize");

            migrationBuilder.RenameColumn(
                name: "SizeDId",
                table: "DrinkSize",
                newName: "SizeId");

            migrationBuilder.RenameIndex(
                name: "IX_DrinkSize_SizeDId",
                table: "DrinkSize",
                newName: "IX_DrinkSize_SizeId");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Pizzas",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Sizes",
                columns: table => new
                {
                    SizeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Quantity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sizes", x => x.SizeId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkSize_Sizes_SizeId",
                table: "DrinkSize",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "SizeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
