using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warmup.App.Migrations
{
    public partial class NullableCashier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashDecks_Users_CashierId",
                table: "CashDecks");

            migrationBuilder.AlterColumn<string>(
                name: "CashierId",
                table: "CashDecks",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_CashDecks_Users_CashierId",
                table: "CashDecks",
                column: "CashierId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashDecks_Users_CashierId",
                table: "CashDecks");

            migrationBuilder.AlterColumn<string>(
                name: "CashierId",
                table: "CashDecks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CashDecks_Users_CashierId",
                table: "CashDecks",
                column: "CashierId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
