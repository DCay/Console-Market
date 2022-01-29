using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warmup.App.Migrations
{
    public partial class CashDeckIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Index",
                table: "CashDecks",
                newName: "CashDeckIndex");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CashDeckIndex",
                table: "CashDecks",
                newName: "Index");
        }
    }
}
