using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBaseTest.Migrations.BankDbContext2Migrations
{
    public partial class EmployeeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Transactions",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Transactions");
        }
    }
}
