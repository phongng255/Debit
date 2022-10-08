using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Debit.Migrations
{
    public partial class adddatecomplete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateComplete",
                table: "DebitCustomer",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateComplete",
                table: "DebitCustomer");
        }
    }
}
