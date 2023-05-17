using Microsoft.EntityFrameworkCore.Migrations;

namespace DesafioWarren.Infrastructure.Migrations
{
    public partial class AddAccountNumberColumn_ToTableAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "Accounts",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Accounts");
        }
    }
}
