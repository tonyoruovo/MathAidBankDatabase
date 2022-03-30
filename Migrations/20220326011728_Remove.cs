using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBaseTest.Migrations
{
    public partial class Remove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Customers_CustomerModelId",
                table: "Accounts");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_CustomerModelId",
                table: "Accounts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BVN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Createdon = table.Column<DateTime>(name: "Created on", type: "DateTime2", nullable: false),
                    Lastupdated = table.Column<DateTime>(name: "Last updated", type: "DateTime2", nullable: false),
                    LoginCredentialModelId = table.Column<int>(type: "int", nullable: true),
                    PersonModelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerModelId);
                    table.ForeignKey(
                        name: "FK_Customers_Login credentials_LoginCredentialModelId",
                        column: x => x.LoginCredentialModelId,
                        principalTable: "Login credentials",
                        principalColumn: "LoginCredentialModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customers_People_PersonModelId",
                        column: x => x.PersonModelId,
                        principalTable: "People",
                        principalColumn: "PersonModelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CustomerModelId",
                table: "Accounts",
                column: "CustomerModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_LoginCredentialModelId",
                table: "Customers",
                column: "LoginCredentialModelId",
                unique: true,
                filter: "[LoginCredentialModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PersonModelId",
                table: "Customers",
                column: "PersonModelId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Customers_CustomerModelId",
                table: "Accounts",
                column: "CustomerModelId",
                principalTable: "Customers",
                principalColumn: "CustomerModelId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
