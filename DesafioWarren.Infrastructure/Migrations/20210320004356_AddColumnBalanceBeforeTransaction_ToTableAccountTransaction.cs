using Microsoft.EntityFrameworkCore.Migrations;

namespace DesafioWarren.Infrastructure.Migrations
{
    public partial class AddColumnBalanceBeforeTransaction_ToTableAccountTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BalanceBeforeTransaction",
                table: "AccountTransaction",
                type: "decimal(19,4)",
                precision: 19,
                scale: 4,
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalanceBeforeTransaction",
                table: "AccountTransaction");
        }
    }
}
