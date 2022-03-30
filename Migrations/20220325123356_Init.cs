using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBaseTest.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Login credentials",
                columns: table => new
                {
                    LoginCredentialModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Personalonlinekey = table.Column<string>(name: "Personal online key", type: "varchar(max)", unicode: false, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tokens = table.Column<string>(name: "Token(s)", type: "nvarchar(max)", nullable: true),
                    Lastupdated = table.Column<DateTime>(name: "Last updated", type: "DateTime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login credentials", x => x.LoginCredentialModelId);
                });

            migrationBuilder.CreateTable(
                name: "Names",
                columns: table => new
                {
                    NameModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(32)", nullable: false),
                    Lastupdated = table.Column<DateTime>(name: "Last updated", type: "DateTime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Names", x => x.NameModelId);
                });

            migrationBuilder.CreateTable(
                name: "FullNames",
                columns: table => new
                {
                    FullNameModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titles = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Othernames = table.Column<string>(name: "Other name(s)", type: "nvarchar(max)", nullable: true),
                    NameModelId = table.Column<int>(type: "int", nullable: false),
                    SurnameNameModelId = table.Column<int>(type: "int", nullable: true),
                    NicknameNameModelId = table.Column<int>(type: "int", nullable: true),
                    MaidenNameNameModelId = table.Column<int>(type: "int", nullable: true),
                    Lastupdated = table.Column<DateTime>(name: "Last updated", type: "DateTime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FullNames", x => x.FullNameModelId);
                    table.ForeignKey(
                        name: "FK_FullNames_Names_MaidenNameNameModelId",
                        column: x => x.MaidenNameNameModelId,
                        principalTable: "Names",
                        principalColumn: "NameModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FullNames_Names_NameModelId",
                        column: x => x.NameModelId,
                        principalTable: "Names",
                        principalColumn: "NameModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FullNames_Names_NicknameNameModelId",
                        column: x => x.NicknameNameModelId,
                        principalTable: "Names",
                        principalColumn: "NameModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FullNames_Names_SurnameNameModelId",
                        column: x => x.SurnameNameModelId,
                        principalTable: "Names",
                        principalColumn: "NameModelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Nationalities",
                columns: table => new
                {
                    NationalityModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    LocalGovernmentArea = table.Column<string>(name: "Local Government Area", type: "nvarchar(max)", nullable: true),
                    CountryNameNameModelId = table.Column<int>(type: "int", nullable: false),
                    StateNameModelId = table.Column<int>(type: "int", nullable: true),
                    CityTownNameModelId = table.Column<int>(type: "int", nullable: true),
                    Lastupdated = table.Column<DateTime>(name: "Last updated", type: "DateTime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nationalities", x => x.NationalityModelId);
                    table.ForeignKey(
                        name: "FK_Nationalities_Names_CityTownNameModelId",
                        column: x => x.CityTownNameModelId,
                        principalTable: "Names",
                        principalColumn: "NameModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nationalities_Names_CountryNameNameModelId",
                        column: x => x.CountryNameNameModelId,
                        principalTable: "Names",
                        principalColumn: "NameModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nationalities_Names_StateNameModelId",
                        column: x => x.StateNameModelId,
                        principalTable: "Names",
                        principalColumn: "NameModelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Streetaddress = table.Column<string>(name: "Street address", type: "nvarchar(450)", nullable: true),
                    ZIPcode = table.Column<string>(name: "Z.I.P code", type: "varchar(max)", unicode: false, nullable: true),
                    PMB = table.Column<string>(name: "P.M.B", type: "varchar(max)", unicode: false, nullable: true),
                    CountryOfResidenceNationalityModelId = table.Column<int>(type: "int", nullable: false),
                    Lastupdated = table.Column<DateTime>(name: "Last updated", type: "DateTime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressModelId);
                    table.ForeignKey(
                        name: "FK_Addresses_Nationalities_CountryOfResidenceNationalityModelId",
                        column: x => x.CountryOfResidenceNationalityModelId,
                        principalTable: "Nationalities",
                        principalColumn: "NationalityModelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ContactModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Emails = table.Column<string>(name: "Email(s)", type: "nvarchar(max)", nullable: true),
                    Socialmedia = table.Column<string>(name: "Social media", type: "nvarchar(max)", nullable: true),
                    Mobiles = table.Column<string>(name: "Mobile(s)", type: "nvarchar(max)", nullable: true),
                    Websites = table.Column<string>(name: "Website(s)", type: "nvarchar(max)", nullable: true),
                    AddressModelId = table.Column<int>(type: "int", nullable: false),
                    Lastupdated = table.Column<DateTime>(name: "Last updated", type: "DateTime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ContactModelId);
                    table.ForeignKey(
                        name: "FK_Contacts_Addresses_AddressModelId",
                        column: x => x.AddressModelId,
                        principalTable: "Addresses",
                        principalColumn: "AddressModelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Entities",
                columns: table => new
                {
                    EntityModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameModelIdId = table.Column<int>(type: "int", nullable: false),
                    ContactModelIdId = table.Column<int>(type: "int", nullable: true),
                    Lastupdated = table.Column<DateTime>(name: "Last updated", type: "DateTime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entities", x => x.EntityModelId);
                    table.ForeignKey(
                        name: "FK_Entities_Contacts_ContactModelIdId",
                        column: x => x.ContactModelIdId,
                        principalTable: "Contacts",
                        principalColumn: "ContactModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entities_Names_NameModelIdId",
                        column: x => x.NameModelIdId,
                        principalTable: "Names",
                        principalColumn: "NameModelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    PersonModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueTag = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaritalStatus = table.Column<int>(type: "int", nullable: false),
                    IsMale = table.Column<bool>(type: "bit", nullable: false),
                    JobType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Passport = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Signature = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    FingerPrint = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Nextofkin = table.Column<string>(name: "Next of kin", type: "nvarchar(max)", nullable: true),
                    Identifications = table.Column<string>(name: "Identification(s)", type: "nvarchar(max)", nullable: true),
                    CountryOfOriginNationalityModelId = table.Column<int>(type: "int", nullable: true),
                    FullNameModelId = table.Column<int>(type: "int", nullable: false),
                    OfficialEntityEntityModelId = table.Column<int>(type: "int", nullable: true),
                    EntityModelId = table.Column<int>(type: "int", nullable: true),
                    Lastupdated = table.Column<DateTime>(name: "Last updated", type: "DateTime2", nullable: false),
                    Birthdate = table.Column<DateTime>(name: "Birth date", type: "DateTime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.PersonModelId);
                    table.ForeignKey(
                        name: "FK_People_Entities_EntityModelId",
                        column: x => x.EntityModelId,
                        principalTable: "Entities",
                        principalColumn: "EntityModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_Entities_OfficialEntityEntityModelId",
                        column: x => x.OfficialEntityEntityModelId,
                        principalTable: "Entities",
                        principalColumn: "EntityModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_FullNames_FullNameModelId",
                        column: x => x.FullNameModelId,
                        principalTable: "FullNames",
                        principalColumn: "FullNameModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_Nationalities_CountryOfOriginNationalityModelId",
                        column: x => x.CountryOfOriginNationalityModelId,
                        principalTable: "Nationalities",
                        principalColumn: "NationalityModelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonModelId = table.Column<int>(type: "int", nullable: false),
                    LoginCredentialModelId = table.Column<int>(type: "int", nullable: true),
                    BVN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Createdon = table.Column<DateTime>(name: "Created on", type: "DateTime2", nullable: false),
                    Lastupdated = table.Column<DateTime>(name: "Last updated", type: "DateTime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuarantorPersonModelId = table.Column<int>(type: "int", nullable: true),
                    CustomerModelId = table.Column<int>(type: "int", nullable: true),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Createdon = table.Column<DateTime>(name: "Created on", type: "DateTime2", nullable: false),
                    Balance = table.Column<decimal>(type: "money", nullable: false),
                    PercentageIncrease = table.Column<decimal>(type: "money", nullable: false),
                    PercentageDecrease = table.Column<decimal>(type: "money", nullable: false),
                    Debt = table.Column<decimal>(type: "money", nullable: false),
                    DebitLimit = table.Column<decimal>(type: "money", nullable: false),
                    CreditLimit = table.Column<decimal>(type: "money", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreditIntervalLimit = table.Column<TimeSpan>(type: "time", nullable: false),
                    DebitIntervalLimit = table.Column<TimeSpan>(type: "time", nullable: false),
                    SmallestBalance = table.Column<decimal>(type: "money", nullable: false),
                    SmallestTransferIn = table.Column<decimal>(type: "money", nullable: false),
                    SmallestTransferOut = table.Column<decimal>(type: "money", nullable: false),
                    LargestBalance = table.Column<decimal>(type: "money", nullable: false),
                    LargestTransferIn = table.Column<decimal>(type: "money", nullable: false),
                    LargestTransferOut = table.Column<decimal>(type: "money", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SMSNumbers = table.Column<string>(name: "SMS Number(s)", type: "nvarchar(max)", nullable: true),
                    Emails = table.Column<string>(name: "Email(s)", type: "nvarchar(max)", nullable: true),
                    Statuses = table.Column<string>(name: "Status(es)", type: "nvarchar(max)", nullable: true),
                    Lastupdated = table.Column<DateTime>(name: "Last updated", type: "DateTime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountModelId);
                    table.ForeignKey(
                        name: "FK_Accounts_Customers_CustomerModelId",
                        column: x => x.CustomerModelId,
                        principalTable: "Customers",
                        principalColumn: "CustomerModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accounts_People_GuarantorPersonModelId",
                        column: x => x.GuarantorPersonModelId,
                        principalTable: "People",
                        principalColumn: "PersonModelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    CardModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SecretLoginCredentialModelId = table.Column<int>(type: "int", nullable: true),
                    BrandEntityModelId = table.Column<int>(type: "int", nullable: false),
                    CountryOfUseNationalityModelId = table.Column<int>(type: "int", nullable: true),
                    AccountModelId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Issuedon = table.Column<DateTime>(name: "Issued on", type: "DateTime2", nullable: false),
                    Expireson = table.Column<DateTime>(name: "Expires on", type: "DateTime2", nullable: false),
                    Issuedcost = table.Column<decimal>(name: "Issued cost", type: "money", nullable: false),
                    Monthlyrate = table.Column<decimal>(name: "Monthly rate", type: "money", nullable: false),
                    Withdrawallimit = table.Column<decimal>(name: "Withdrawal limit", type: "money", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastupdated = table.Column<DateTime>(name: "Last updated", type: "DateTime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardModelId);
                    table.ForeignKey(
                        name: "FK_Cards_Accounts_AccountModelId",
                        column: x => x.AccountModelId,
                        principalTable: "Accounts",
                        principalColumn: "AccountModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cards_Entities_BrandEntityModelId",
                        column: x => x.BrandEntityModelId,
                        principalTable: "Entities",
                        principalColumn: "EntityModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cards_Login credentials_SecretLoginCredentialModelId",
                        column: x => x.SecretLoginCredentialModelId,
                        principalTable: "Login credentials",
                        principalColumn: "LoginCredentialModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cards_Nationalities_CountryOfUseNationalityModelId",
                        column: x => x.CountryOfUseNationalityModelId,
                        principalTable: "Nationalities",
                        principalColumn: "NationalityModelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditorEntityModelId = table.Column<int>(type: "int", nullable: false),
                    DebitorEntityModelId = table.Column<int>(type: "int", nullable: true),
                    AccountModelId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: false),
                    IsIncoming = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Transactiondate = table.Column<DateTime>(name: "Transaction date", type: "DateTime2", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    TransactionGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Lastupdated = table.Column<DateTime>(name: "Last updated", type: "DateTime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionModelId);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_AccountModelId",
                        column: x => x.AccountModelId,
                        principalTable: "Accounts",
                        principalColumn: "AccountModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Entities_CreditorEntityModelId",
                        column: x => x.CreditorEntityModelId,
                        principalTable: "Entities",
                        principalColumn: "EntityModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Entities_DebitorEntityModelId",
                        column: x => x.DebitorEntityModelId,
                        principalTable: "Entities",
                        principalColumn: "EntityModelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Qualifications",
                columns: table => new
                {
                    QualificationModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Grades = table.Column<string>(name: "Grade(s)", type: "nvarchar(max)", nullable: true),
                    Certification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademyAddressModelId = table.Column<int>(type: "int", nullable: true),
                    EducationModelId = table.Column<int>(type: "int", nullable: true),
                    Lastupdated = table.Column<DateTime>(name: "Last updated", type: "DateTime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualifications", x => x.QualificationModelId);
                    table.ForeignKey(
                        name: "FK_Qualifications_Addresses_AcademyAddressModelId",
                        column: x => x.AcademyAddressModelId,
                        principalTable: "Addresses",
                        principalColumn: "AddressModelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Education credentials",
                columns: table => new
                {
                    EducationModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrimaryQualificationModelId = table.Column<int>(type: "int", nullable: true),
                    SecondaryQualificationModelId = table.Column<int>(type: "int", nullable: true),
                    PrimaryTertiaryQualificationModelId = table.Column<int>(type: "int", nullable: false),
                    Lastupdated = table.Column<DateTime>(name: "Last updated", type: "DateTime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Education credentials", x => x.EducationModelId);
                    table.ForeignKey(
                        name: "FK_Education credentials_Qualifications_PrimaryQualificationModelId",
                        column: x => x.PrimaryQualificationModelId,
                        principalTable: "Qualifications",
                        principalColumn: "QualificationModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Education credentials_Qualifications_PrimaryTertiaryQualificationModelId",
                        column: x => x.PrimaryTertiaryQualificationModelId,
                        principalTable: "Qualifications",
                        principalColumn: "QualificationModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Education credentials_Qualifications_SecondaryQualificationModelId",
                        column: x => x.SecondaryQualificationModelId,
                        principalTable: "Qualifications",
                        principalColumn: "QualificationModelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salary = table.Column<decimal>(type: "money", nullable: false),
                    Group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Supervisor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Superior = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subordinate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hiredate = table.Column<DateTime>(name: "Hire date", type: "DateTime2", nullable: false),
                    WorkingStatus = table.Column<int>(type: "int", nullable: false),
                    PersonModelId = table.Column<int>(type: "int", nullable: false),
                    GuarantorPersonModelId = table.Column<int>(type: "int", nullable: true),
                    SecretLoginCredentialModelId = table.Column<int>(type: "int", nullable: true),
                    QualificationEducationModelId = table.Column<int>(type: "int", nullable: true),
                    Lastupdated = table.Column<DateTime>(name: "Last updated", type: "DateTime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeModelId);
                    table.ForeignKey(
                        name: "FK_Employees_Education credentials_QualificationEducationModelId",
                        column: x => x.QualificationEducationModelId,
                        principalTable: "Education credentials",
                        principalColumn: "EducationModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Login credentials_SecretLoginCredentialModelId",
                        column: x => x.SecretLoginCredentialModelId,
                        principalTable: "Login credentials",
                        principalColumn: "LoginCredentialModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_People_GuarantorPersonModelId",
                        column: x => x.GuarantorPersonModelId,
                        principalTable: "People",
                        principalColumn: "PersonModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_People_PersonModelId",
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
                name: "IX_Accounts_GuarantorPersonModelId",
                table: "Accounts",
                column: "GuarantorPersonModelId",
                unique: true,
                filter: "[GuarantorPersonModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountryOfResidenceNationalityModelId",
                table: "Addresses",
                column: "CountryOfResidenceNationalityModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Street address",
                table: "Addresses",
                column: "Street address");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_AccountModelId",
                table: "Cards",
                column: "AccountModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_BrandEntityModelId",
                table: "Cards",
                column: "BrandEntityModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CountryOfUseNationalityModelId",
                table: "Cards",
                column: "CountryOfUseNationalityModelId",
                unique: true,
                filter: "[CountryOfUseNationalityModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_SecretLoginCredentialModelId",
                table: "Cards",
                column: "SecretLoginCredentialModelId",
                unique: true,
                filter: "[SecretLoginCredentialModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_AddressModelId",
                table: "Contacts",
                column: "AddressModelId",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Education credentials_PrimaryQualificationModelId",
                table: "Education credentials",
                column: "PrimaryQualificationModelId",
                unique: true,
                filter: "[PrimaryQualificationModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Education credentials_PrimaryTertiaryQualificationModelId",
                table: "Education credentials",
                column: "PrimaryTertiaryQualificationModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Education credentials_SecondaryQualificationModelId",
                table: "Education credentials",
                column: "SecondaryQualificationModelId",
                unique: true,
                filter: "[SecondaryQualificationModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_GuarantorPersonModelId",
                table: "Employees",
                column: "GuarantorPersonModelId",
                unique: true,
                filter: "[GuarantorPersonModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PersonModelId",
                table: "Employees",
                column: "PersonModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_QualificationEducationModelId",
                table: "Employees",
                column: "QualificationEducationModelId",
                unique: true,
                filter: "[QualificationEducationModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SecretLoginCredentialModelId",
                table: "Employees",
                column: "SecretLoginCredentialModelId",
                unique: true,
                filter: "[SecretLoginCredentialModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_ContactModelIdId",
                table: "Entities",
                column: "ContactModelIdId",
                unique: true,
                filter: "[ContactModelIdId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_NameModelIdId",
                table: "Entities",
                column: "NameModelIdId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FullNames_MaidenNameNameModelId",
                table: "FullNames",
                column: "MaidenNameNameModelId",
                unique: true,
                filter: "[MaidenNameNameModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FullNames_NameModelId",
                table: "FullNames",
                column: "NameModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FullNames_NicknameNameModelId",
                table: "FullNames",
                column: "NicknameNameModelId",
                unique: true,
                filter: "[NicknameNameModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FullNames_SurnameNameModelId",
                table: "FullNames",
                column: "SurnameNameModelId",
                unique: true,
                filter: "[SurnameNameModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_CityTownNameModelId",
                table: "Nationalities",
                column: "CityTownNameModelId",
                unique: true,
                filter: "[CityTownNameModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_CountryNameNameModelId",
                table: "Nationalities",
                column: "CountryNameNameModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_StateNameModelId",
                table: "Nationalities",
                column: "StateNameModelId",
                unique: true,
                filter: "[StateNameModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_People_CountryOfOriginNationalityModelId",
                table: "People",
                column: "CountryOfOriginNationalityModelId",
                unique: true,
                filter: "[CountryOfOriginNationalityModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_People_EntityModelId",
                table: "People",
                column: "EntityModelId",
                unique: true,
                filter: "[EntityModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_People_FullNameModelId",
                table: "People",
                column: "FullNameModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_People_OfficialEntityEntityModelId",
                table: "People",
                column: "OfficialEntityEntityModelId",
                unique: true,
                filter: "[OfficialEntityEntityModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Qualifications_AcademyAddressModelId",
                table: "Qualifications",
                column: "AcademyAddressModelId",
                unique: true,
                filter: "[AcademyAddressModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Qualifications_EducationModelId",
                table: "Qualifications",
                column: "EducationModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountModelId",
                table: "Transactions",
                column: "AccountModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CreditorEntityModelId",
                table: "Transactions",
                column: "CreditorEntityModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_DebitorEntityModelId",
                table: "Transactions",
                column: "DebitorEntityModelId",
                unique: true,
                filter: "[DebitorEntityModelId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Qualifications_Education credentials_EducationModelId",
                table: "Qualifications",
                column: "EducationModelId",
                principalTable: "Education credentials",
                principalColumn: "EducationModelId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Nationalities_CountryOfResidenceNationalityModelId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Qualifications_Addresses_AcademyAddressModelId",
                table: "Qualifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Education credentials_Qualifications_PrimaryQualificationModelId",
                table: "Education credentials");

            migrationBuilder.DropForeignKey(
                name: "FK_Education credentials_Qualifications_PrimaryTertiaryQualificationModelId",
                table: "Education credentials");

            migrationBuilder.DropForeignKey(
                name: "FK_Education credentials_Qualifications_SecondaryQualificationModelId",
                table: "Education credentials");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Login credentials");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Entities");

            migrationBuilder.DropTable(
                name: "FullNames");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Nationalities");

            migrationBuilder.DropTable(
                name: "Names");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Qualifications");

            migrationBuilder.DropTable(
                name: "Education credentials");
        }
    }
}
