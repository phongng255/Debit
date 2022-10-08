using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Debit.Migrations
{
    public partial class addMoneyProcess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ProcessMoney",
                table: "DebitCustomer",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessMoney",
                table: "DebitCustomer");
        }
    }
}
