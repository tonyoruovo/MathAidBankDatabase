using DataBaseTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseTest.Data
{
    internal class EmployeeConfig : IEntityTypeConfiguration<EmployeeModel>
    {
        public void Configure(EntityTypeBuilder<EmployeeModel> entity)
        {
            entity.Property<DateTime>("LastUpdatedOn")
                .HasColumnName("Last updated")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());

            entity.HasKey(n => n.EmployeeModelId);
            entity.ToTable("Employees");

            entity.HasOne(e => e.Person)
                .WithOne()
                .IsRequired()
                //.HasForeignKey(nameof(EmployeeModel.PersonModelId))
                .OnDelete(DeleteBehavior.ClientCascade);

            entity.HasOne(e => e.Guarantor)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(EmployeeModel.GuarantorPersonModelId))
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(e => e.Secret)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(EmployeeModel.SecretLoginCredentialModelId))
                .OnDelete(DeleteBehavior.ClientSetNull);

            //entity.OwnsOne(e => e.Qualification).WithOwner();

            entity.HasOne(e => e.Qualification)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(QualificationModel.AcademyAddressModelId))
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.Property(n => n.Salary)
                .HasColumnName("Salary")
                .HasColumnType<decimal>("money");

            entity.Property(e => e.HireDate)
                .HasColumnName("Hire date")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());

            entity.Property(e => e.Group)
                .HasConversion(Utilities.JsonStringConverter3<List<EmployeeModel>, string>(), Utilities.ListComparer<EmployeeModel>())
                .HasColumnName("Group")
                .IsRequired(false);

            entity.Property(e => e.Supervisor)
                .HasConversion(Utilities.JsonStringConverter3<EmployeeModel, string>(), Utilities.Comparer<EmployeeModel>())
                .HasColumnName("Supervisor")
                .IsRequired(false);

            entity.Property(e => e.Superior)
                .HasConversion(Utilities.JsonStringConverter3<EmployeeModel, string>(), Utilities.Comparer<EmployeeModel>())
                .HasColumnName("Superior")
                .IsRequired(false);

            entity.Property(e => e.Subordinate)
                .HasConversion(Utilities.JsonStringConverter3<EmployeeModel, string>(), Utilities.Comparer<EmployeeModel>())
                .HasColumnName("Subordinate")
                .IsRequired(false);

            entity.Property(e => e.Account)
                .HasConversion(Utilities.JsonStringConverter3<AccountModel, string>(), Utilities.Comparer<AccountModel>())
                .HasColumnName("Account")
                .IsRequired(false);

        }
    }
}
