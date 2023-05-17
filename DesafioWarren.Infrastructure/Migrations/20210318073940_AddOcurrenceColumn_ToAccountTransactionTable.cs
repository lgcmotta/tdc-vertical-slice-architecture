using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DesafioWarren.Infrastructure.Migrations
{
    public partial class AddOcurrenceColumn_ToAccountTransactionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Occurrence",
                table: "AccountTransaction",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Occurrence",
                table: "AccountTransaction");
        }
    }
}
