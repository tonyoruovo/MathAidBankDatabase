using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBaseTest.Migrations
{
    public partial class AddressIsNowNullableInDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contacts_AddressModelId",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_CountryOfResidenceNationalityModelId",
                table: "Addresses");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Mobile(s)",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AddressModelId",
                table: "Contacts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Street address",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CountryOfResidenceNationalityModelId",
                table: "Addresses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_AddressModelId",
                table: "Contacts",
                column: "AddressModelId",
                unique: true,
                filter: "[AddressModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountryOfResidenceNationalityModelId",
                table: "Addresses",
                column: "CountryOfResidenceNationalityModelId",
                unique: true,
                filter: "[CountryOfResidenceNationalityModelId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contacts_AddressModelId",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_CountryOfResidenceNationalityModelId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Transactions");

            migrationBuilder.AlterColumn<string>(
                name: "Mobile(s)",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "AddressModelId",
                table: "Contacts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Street address",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "CountryOfResidenceNationalityModelId",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_AddressModelId",
                table: "Contacts",
                column: "AddressModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountryOfResidenceNationalityModelId",
                table: "Addresses",
                column: "CountryOfResidenceNationalityModelId",
                unique: true);
        }
    }
}
