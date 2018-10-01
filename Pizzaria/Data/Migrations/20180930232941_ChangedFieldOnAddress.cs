using Microsoft.EntityFrameworkCore.Migrations;

namespace Pizzaria.Migrations
{
    public partial class ChangedFieldOnAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Addresses",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "Addresses",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
