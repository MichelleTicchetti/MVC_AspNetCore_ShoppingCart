using Microsoft.EntityFrameworkCore.Migrations;

namespace Carrito_A.Migrations
{
    public partial class valorunitarioCarrito : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ValorUnitario",
                table: "CarritoItems",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorUnitario",
                table: "CarritoItems");
        }
    }
}
