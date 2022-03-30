﻿// <auto-generated />
using System;
using DataBaseTest.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataBaseTest.Migrations
{
    [DbContext(typeof(BankDbContext))]
    partial class BankDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataBaseTest.Models.AccountModel", b =>
                {
                    b.Property<int>("AccountModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Balance")
                        .HasColumnType("money")
                        .HasColumnName("Balance");

                    b.Property<TimeSpan>("CreditIntervalLimit")
                        .HasColumnType("time");

                    b.Property<decimal>("CreditLimit")
                        .HasColumnType("money");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Currency");

                    b.Property<int?>("CustomerModelId")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("DebitIntervalLimit")
                        .HasColumnType("time");

                    b.Property<decimal>("DebitLimit")
                        .HasColumnType("money");

                    b.Property<decimal>("Debt")
                        .HasColumnType("money");

                    b.Property<string>("EmailAlertList")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Email(s)");

                    b.Property<DateTime>("EntryDate")
                        .HasColumnType("DateTime2")
                        .HasColumnName("Created on");

                    b.Property<int?>("GuarantorPersonModelId")
                        .HasColumnType("int");

                    b.Property<decimal>("LargestBalance")
                        .HasColumnType("money");

                    b.Property<decimal>("LargestTransferIn")
                        .HasColumnType("money");

                    b.Property<decimal>("LargestTransferOut")
                        .HasColumnType("money");

                    b.Property<DateTime>("LastUpdatedOn")
                        .HasColumnType("DateTime2")
                        .HasColumnName("Last updated");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PercentageDecrease")
                        .HasColumnType("money");

                    b.Property<decimal>("PercentageIncrease")
                        .HasColumnType("money");

                    b.Property<string>("SMSAlertList")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("SMS Number(s)");

                    b.Property<decimal>("SmallestBalance")
                        .HasColumnType("money");

                    b.Property<decimal>("SmallestTransferIn")
                        .HasColumnType("money");

                    b.Property<decimal>("SmallestTransferOut")
                        .HasColumnType("money");

                    b.Property<string>("Statuses")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Status(es)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("AccountModelId");

                    b.HasIndex("GuarantorPersonModelId")
                        .IsUnique()
                        .HasFilter("[GuarantorPersonModelId] IS NOT NULL");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("DataBaseTest.Models.AddressModel", b =>
                {
                    b.Property<int?>("AddressModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CountryOfResidenceNationalityModelId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdatedOn")
                        .HasColumnType("DateTime2")
                        .HasColumnName("Last updated");

                    b.Property<string>("PMB")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)")
                        .HasColumnName("P.M.B");

                    b.Property<string>("StreetAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("Street address");

                    b.Property<string>("ZIPCode")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)")
                        .HasColumnName("Z.I.P code");

                    b.HasKey("AddressModelId");

                    b.HasIndex("CountryOfResidenceNationalityModelId")
                        .IsUnique()
                        .HasFilter("[CountryOfResidenceNationalityModelId] IS NOT NULL");

                    b.HasIndex("StreetAddress");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("DataBaseTest.Models.CardModel", b =>
                {
                    b.Property<int?>("CardModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AccountModelId")
                        .HasColumnType("int");

                    b.Property<int?>("BrandEntityModelId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("CountryOfUseNationalityModelId")
                        .HasColumnType("int");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Currency");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("DateTime2")
                        .HasColumnName("Expires on");

                    b.Property<decimal>("IssuedCost")
                        .HasColumnType("money")
                        .HasColumnName("Issued cost");

                    b.Property<DateTime>("IssuedDate")
                        .HasColumnType("DateTime2")
                        .HasColumnName("Issued on");

                    b.Property<DateTime>("LastUpdatedOn")
                        .HasColumnType("DateTime2")
                        .HasColumnName("Last updated");

                    b.Property<decimal>("MonthlyRate")
                        .HasColumnType("money")
                        .HasColumnName("Monthly rate");

                    b.Property<int?>("SecretLoginCredentialModelId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<decimal>("WithdrawalLimit")
                        .HasColumnType("money")
                        .HasColumnName("Withdrawal limit");

                    b.HasKey("CardModelId");

                    b.HasIndex("AccountModelId");

                    b.HasIndex("BrandEntityModelId")
                        .IsUnique();

                    b.HasIndex("CountryOfUseNationalityModelId")
                        .IsUnique()
                        .HasFilter("[CountryOfUseNationalityModelId] IS NOT NULL");

                    b.HasIndex("SecretLoginCredentialModelId")
                        .IsUnique()
                        .HasFilter("[SecretLoginCredentialModelId] IS NOT NULL");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("DataBaseTest.Models.ContactModel", b =>
                {
                    b.Property<int?>("ContactModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AddressModelId")
                        .HasColumnType("int");

                    b.Property<string>("Emails")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Email(s)");

                    b.Property<DateTime>("LastUpdatedOn")
                        .HasColumnType("DateTime2")
                        .HasColumnName("Last updated");

                    b.Property<string>("Mobiles")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Mobile(s)");

                    b.Property<string>("SocialMedia")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Social media");

                    b.Property<string>("Websites")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Website(s)");

                    b.HasKey("ContactModelId");

                    b.HasIndex("AddressModelId")
                        .IsUnique()
                        .HasFilter("[AddressModelId] IS NOT NULL");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("DataBaseTest.Models.EducationModel", b =>
                {
                    b.Property<int?>("EducationModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("LastUpdatedOn")
                        .HasColumnType("DateTime2")
                        .HasColumnName("Last updated");

                    b.Property<int?>("PrimaryQualificationModelId")
                        .HasColumnType("int");

                    b.Property<int?>("PrimaryTertiaryQualificationModelId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("SecondaryQualificationModelId")
                        .HasColumnType("int");

                    b.HasKey("EducationModelId");

                    b.HasIndex("PrimaryQualificationModelId")
                        .IsUnique()
                        .HasFilter("[PrimaryQualificationModelId] IS NOT NULL");

                    b.HasIndex("PrimaryTertiaryQualificationModelId")
                        .IsUnique();

                    b.HasIndex("SecondaryQualificationModelId")
                        .IsUnique()
                        .HasFilter("[SecondaryQualificationModelId] IS NOT NULL");

                    b.ToTable("Education credentials");
                });

            modelBuilder.Entity("DataBaseTest.Models.EmployeeModel", b =>
                {
                    b.Property<int?>("EmployeeModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Account")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Account");

                    b.Property<string>("Group")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Group");

                    b.Property<int?>("GuarantorPersonModelId")
                        .HasColumnType("int");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("DateTime2")
                        .HasColumnName("Hire date");

                    b.Property<DateTime>("LastUpdatedOn")
                        .HasColumnType("DateTime2")
                        .HasColumnName("Last updated");

                    b.Property<string>("Level")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PersonModelId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("QualificationEducationModelId")
                        .HasColumnType("int");

                    b.Property<decimal>("Salary")
                        .HasColumnType("money")
                        .HasColumnName("Salary");

                    b.Property<int?>("SecretLoginCredentialModelId")
                        .HasColumnType("int");

                    b.Property<string>("Subordinate")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Subordinate");

                    b.Property<string>("Superior")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Superior");

                    b.Property<string>("Supervisor")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Supervisor");

                    b.Property<int>("WorkingStatus")
                        .HasColumnType("int");

                    b.HasKey("EmployeeModelId");

                    b.HasIndex("GuarantorPersonModelId")
                        .IsUnique()
                        .HasFilter("[GuarantorPersonModelId] IS NOT NULL");

                    b.HasIndex("PersonModelId")
                        .IsUnique();

                    b.HasIndex("QualificationEducationModelId")
                        .IsUnique()
                        .HasFilter("[QualificationEducationModelId] IS NOT NULL");

                    b.HasIndex("SecretLoginCredentialModelId")
                        .IsUnique()
                        .HasFilter("[SecretLoginCredentialModelId] IS NOT NULL");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("DataBaseTest.Models.EntityModel", b =>
                {
                    b.Property<int?>("EntityModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ContactModelIdId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdatedOn")
                        .HasColumnType("DateTime2")
                        .HasColumnName("Last updated");

                    b.Property<int?>("NameModelIdId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("EntityModelId");

                    b.HasIndex("ContactModelIdId")
                        .IsUnique()
                        .HasFilter("[ContactModelIdId] IS NOT NULL");

                    b.HasIndex("NameModelIdId")
                        .IsUnique();

                    b.ToTable("Entities");
                });

            modelBuilder.Entity("DataBaseTest.Models.FullNameModel", b =>
                {
                    b.Property<int?>("FullNameModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("LastUpdatedOn")
                        .HasColumnType("DateTime2")
                        .HasColumnName("Last updated");

                    b.Property<int?>("MaidenNameNameModelId")
                        .HasColumnType("int");

                    b.Property<int?>("NameModelId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("NicknameNameModelId")
                        .HasColumnType("int");

                    b.Property<string>("OtherNames")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Other name(s)");

                    b.Property<int?>("SurnameNameModelId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Titles");

                    b.HasKey("FullNameModelId");

                    b.HasIndex("MaidenNameNameModelId")
                        .IsUnique()
                        .HasFilter("[MaidenNameNameModelId] IS NOT NULL");

                    b.HasIndex("NameModelId")
                        .IsUnique();

                    b.HasIndex("NicknameNameModelId")
                        .IsUnique()
                        .HasFilter("[NicknameNameModelId] IS NOT NULL");

                    b.HasIndex("SurnameNameModelId")
                        .IsUnique()
                        .HasFilter("[SurnameNameModelId] IS NOT NULL");

                    b.ToTable("FullNames");
                });

            modelBuilder.Entity("DataBaseTest.Models.LoginCredentialModel", b =>
                {
                    b.Property<int?>("LoginCredentialModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("LastUpdatedOn")
                        .HasColumnType("DateTime2")
                        .HasColumnName("Last updated");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Password");

                    b.Property<string>("PersonalOnlineKey")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)")
                        .HasColumnName("Personal online key");

                    b.Property<string>("Tokens")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Token(s)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Username");

                    b.HasKey("LoginCredentialModelId");

                    b.ToTable("Login credentials");
                });

            modelBuilder.Entity("DataBaseTest.Models.NameModel", b =>
                {
                    b.Property<int?>("NameModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("LastUpdatedOn")
                        .HasColumnType("DateTime2")
                        .HasColumnName("Last updated");

                    b.Property<string>("Name")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("NVARCHAR(32)")
                        .HasColumnName("Name");

                    b.HasKey("NameModelId");

                    b.ToTable("Names");
                });

            modelBuilder.Entity("DataBaseTest.Models.NationalityModel", b =>
                {
                    b.Property<int?>("NationalityModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CityTownNameModelId")
                        .HasColumnType("int");

                    b.Property<int?>("CountryNameNameModelId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("LGA")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Local Government Area");

                    b.Property<string>("Language")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)")
                        .HasColumnName("Language");

                    b.Property<DateTime>("LastUpdatedOn")
                        .HasColumnType("DateTime2")
                        .HasColumnName("Last updated");

                    b.Property<int?>("StateNameModelId")
                        .HasColumnType("int");

                    b.HasKey("NationalityModelId");

                    b.HasIndex("CityTownNameModelId")
                        .IsUnique()
                        .HasFilter("[CityTownNameModelId] IS NOT NULL");

                    b.HasIndex("CountryNameNameModelId")
                        .IsUnique();

                    b.HasIndex("StateNameModelId")
                        .IsUnique()
                        .HasFilter("[StateNameModelId] IS NOT NULL");

                    b.ToTable("Nationalities");
                });

            modelBuilder.Entity("DataBaseTest.Models.PersonModel", b =>
                {
                    b.Property<int?>("PersonModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CountryOfOriginNationalityModelId")
                        .HasColumnType("int");

                    b.Property<int?>("EntityModelId")
                        .HasColumnType("int");

                    b.Property<byte[]>("FingerPrint")
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("FullNameModelId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("Identification")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Identification(s)");

                    b.Property<bool>("IsMale")
                        .HasColumnType("bit");

                    b.Property<string>("JobType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdatedOn")
                        .HasColumnType("DateTime2")
                        .HasColumnName("Last updated");

                    b.Property<int>("MaritalStatus")
                        .HasColumnType("int");

                    b.Property<string>("NextOfKin")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Next of kin");

                    b.Property<int?>("OfficialEntityEntityModelId")
                        .HasColumnType("int");

                    b.Property<byte[]>("Passport")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("Signature")
                        .HasColumnType("varbinary(max)");

                    b.Property<Guid>("UniqueTag")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("_birthDate")
                        .HasColumnType("DateTime2")
                        .HasColumnName("Birth date");

                    b.HasKey("PersonModelId");

                    b.HasIndex("CountryOfOriginNationalityModelId")
                        .IsUnique()
                        .HasFilter("[CountryOfOriginNationalityModelId] IS NOT NULL");

                    b.HasIndex("EntityModelId")
                        .IsUnique()
                        .HasFilter("[EntityModelId] IS NOT NULL");

                    b.HasIndex("FullNameModelId")
                        .IsUnique();

                    b.HasIndex("OfficialEntityEntityModelId")
                        .IsUnique()
                        .HasFilter("[OfficialEntityEntityModelId] IS NOT NULL");

                    b.ToTable("People");
                });

            modelBuilder.Entity("DataBaseTest.Models.QualificationModel", b =>
                {
                    b.Property<int?>("QualificationModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AcademyAddressModelId")
                        .HasColumnType("int");

                    b.Property<string>("Certification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EducationModelId")
                        .HasColumnType("int");

                    b.Property<string>("Grades")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Grade(s)");

                    b.Property<DateTime>("LastUpdatedOn")
                        .HasColumnType("DateTime2")
                        .HasColumnName("Last updated");

                    b.HasKey("QualificationModelId");

                    b.HasIndex("AcademyAddressModelId")
                        .IsUnique()
                        .HasFilter("[AcademyAddressModelId] IS NOT NULL");

                    b.HasIndex("EducationModelId");

                    b.ToTable("Qualifications");
                });

            modelBuilder.Entity("DataBaseTest.Models.TransactionModel", b =>
                {
                    b.Property<int?>("TransactionModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AccountModelId")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("money")
                        .HasColumnName("Amount");

                    b.Property<int?>("CreditorEntityModelId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Currency");

                    b.Property<DateTime>("Date")
                        .HasColumnType("DateTime2")
                        .HasColumnName("Transaction date");

                    b.Property<int?>("DebitorEntityModelId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<bool>("IsIncoming")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastUpdatedOn")
                        .HasColumnType("DateTime2")
                        .HasColumnName("Last updated");

                    b.Property<Guid>("TransactionGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TransactionType")
                        .HasColumnType("int");

                    b.HasKey("TransactionModelId");

                    b.HasIndex("AccountModelId");

                    b.HasIndex("CreditorEntityModelId")
                        .IsUnique();

                    b.HasIndex("DebitorEntityModelId")
                        .IsUnique()
                        .HasFilter("[DebitorEntityModelId] IS NOT NULL");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("DataBaseTest.Models.AccountModel", b =>
                {
                    b.HasOne("DataBaseTest.Models.PersonModel", "Guarantor")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.AccountModel", "GuarantorPersonModelId");

                    b.Navigation("Guarantor");
                });

            modelBuilder.Entity("DataBaseTest.Models.AddressModel", b =>
                {
                    b.HasOne("DataBaseTest.Models.NationalityModel", "CountryOfResidence")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.AddressModel", "CountryOfResidenceNationalityModelId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("CountryOfResidence");
                });

            modelBuilder.Entity("DataBaseTest.Models.CardModel", b =>
                {
                    b.HasOne("DataBaseTest.Models.AccountModel", null)
                        .WithMany("Cards")
                        .HasForeignKey("AccountModelId");

                    b.HasOne("DataBaseTest.Models.EntityModel", "Brand")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.CardModel", "BrandEntityModelId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("DataBaseTest.Models.NationalityModel", "CountryOfUse")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.CardModel", "CountryOfUseNationalityModelId");

                    b.HasOne("DataBaseTest.Models.LoginCredentialModel", "Secret")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.CardModel", "SecretLoginCredentialModelId");

                    b.Navigation("Brand");

                    b.Navigation("CountryOfUse");

                    b.Navigation("Secret");
                });

            modelBuilder.Entity("DataBaseTest.Models.ContactModel", b =>
                {
                    b.HasOne("DataBaseTest.Models.AddressModel", "Address")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.ContactModel", "AddressModelId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("Address");
                });

            modelBuilder.Entity("DataBaseTest.Models.EducationModel", b =>
                {
                    b.HasOne("DataBaseTest.Models.QualificationModel", "Primary")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.EducationModel", "PrimaryQualificationModelId");

                    b.HasOne("DataBaseTest.Models.QualificationModel", "PrimaryTertiary")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.EducationModel", "PrimaryTertiaryQualificationModelId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("DataBaseTest.Models.QualificationModel", "Secondary")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.EducationModel", "SecondaryQualificationModelId");

                    b.Navigation("Primary");

                    b.Navigation("PrimaryTertiary");

                    b.Navigation("Secondary");
                });

            modelBuilder.Entity("DataBaseTest.Models.EmployeeModel", b =>
                {
                    b.HasOne("DataBaseTest.Models.PersonModel", "Guarantor")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.EmployeeModel", "GuarantorPersonModelId");

                    b.HasOne("DataBaseTest.Models.PersonModel", "Person")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.EmployeeModel", "PersonModelId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("DataBaseTest.Models.EducationModel", "Qualification")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.EmployeeModel", "QualificationEducationModelId");

                    b.HasOne("DataBaseTest.Models.LoginCredentialModel", "Secret")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.EmployeeModel", "SecretLoginCredentialModelId");

                    b.Navigation("Guarantor");

                    b.Navigation("Person");

                    b.Navigation("Qualification");

                    b.Navigation("Secret");
                });

            modelBuilder.Entity("DataBaseTest.Models.EntityModel", b =>
                {
                    b.HasOne("DataBaseTest.Models.ContactModel", "Contact")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.EntityModel", "ContactModelIdId");

                    b.HasOne("DataBaseTest.Models.NameModel", "Name")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.EntityModel", "NameModelIdId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Contact");

                    b.Navigation("Name");
                });

            modelBuilder.Entity("DataBaseTest.Models.FullNameModel", b =>
                {
                    b.HasOne("DataBaseTest.Models.NameModel", "MaidenName")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.FullNameModel", "MaidenNameNameModelId");

                    b.HasOne("DataBaseTest.Models.NameModel", "Name")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.FullNameModel", "NameModelId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("DataBaseTest.Models.NameModel", "Nickname")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.FullNameModel", "NicknameNameModelId");

                    b.HasOne("DataBaseTest.Models.NameModel", "Surname")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.FullNameModel", "SurnameNameModelId");

                    b.Navigation("MaidenName");

                    b.Navigation("Name");

                    b.Navigation("Nickname");

                    b.Navigation("Surname");
                });

            modelBuilder.Entity("DataBaseTest.Models.NationalityModel", b =>
                {
                    b.HasOne("DataBaseTest.Models.NameModel", "CityTown")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.NationalityModel", "CityTownNameModelId");

                    b.HasOne("DataBaseTest.Models.NameModel", "CountryName")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.NationalityModel", "CountryNameNameModelId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("DataBaseTest.Models.NameModel", "State")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.NationalityModel", "StateNameModelId");

                    b.Navigation("CityTown");

                    b.Navigation("CountryName");

                    b.Navigation("State");
                });

            modelBuilder.Entity("DataBaseTest.Models.PersonModel", b =>
                {
                    b.HasOne("DataBaseTest.Models.NationalityModel", "CountryOfOrigin")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.PersonModel", "CountryOfOriginNationalityModelId");

                    b.HasOne("DataBaseTest.Models.EntityModel", "Entity")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.PersonModel", "EntityModelId");

                    b.HasOne("DataBaseTest.Models.FullNameModel", "FullName")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.PersonModel", "FullNameModelId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("DataBaseTest.Models.EntityModel", "OfficialEntity")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.PersonModel", "OfficialEntityEntityModelId");

                    b.Navigation("CountryOfOrigin");

                    b.Navigation("Entity");

                    b.Navigation("FullName");

                    b.Navigation("OfficialEntity");
                });

            modelBuilder.Entity("DataBaseTest.Models.QualificationModel", b =>
                {
                    b.HasOne("DataBaseTest.Models.AddressModel", "Academy")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.QualificationModel", "AcademyAddressModelId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.HasOne("DataBaseTest.Models.EducationModel", null)
                        .WithMany("Others")
                        .HasForeignKey("EducationModelId");

                    b.Navigation("Academy");
                });

            modelBuilder.Entity("DataBaseTest.Models.TransactionModel", b =>
                {
                    b.HasOne("DataBaseTest.Models.AccountModel", null)
                        .WithMany("Transactions")
                        .HasForeignKey("AccountModelId");

                    b.HasOne("DataBaseTest.Models.EntityModel", "Creditor")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.TransactionModel", "CreditorEntityModelId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("DataBaseTest.Models.EntityModel", "Debitor")
                        .WithOne()
                        .HasForeignKey("DataBaseTest.Models.TransactionModel", "DebitorEntityModelId");

                    b.Navigation("Creditor");

                    b.Navigation("Debitor");
                });

            modelBuilder.Entity("DataBaseTest.Models.AccountModel", b =>
                {
                    b.Navigation("Cards");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("DataBaseTest.Models.EducationModel", b =>
                {
                    b.Navigation("Others");
                });
#pragma warning restore 612, 618
        }
    }
}
