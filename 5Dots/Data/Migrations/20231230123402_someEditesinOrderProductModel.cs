using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _5Dots.Data.Migrations
{
    public partial class someEditesinOrderProductModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "OrderProduct",
                newName: "ProductQuantity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductQuantity",
                table: "OrderProduct",
                newName: "Quantity");
        }
    }
}
