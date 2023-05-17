using Microsoft.EntityFrameworkCore.Migrations;

namespace DesafioWarren.Infrastructure.Migrations
{
    public partial class AddCurrencyColumn_ToTableAccountBalance_AndCpfColumn_ToAccountsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "Accounts",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "AccountBalance",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "AccountBalance");
        }
    }
}
