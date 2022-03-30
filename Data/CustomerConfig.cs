using DataBaseTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseTest.Data
{
    internal class CustomerConfig : IEntityTypeConfiguration<CustomerModel>
    {
        public void Configure(EntityTypeBuilder<CustomerModel> entity)
        {
            entity.Property<DateTime>("LastUpdatedOn")
                .HasColumnName("Last updated")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());

            entity.HasKey(n => n.CustomerModelId);
            entity.ToTable("Customers");

            entity.HasMany(t => t.Accounts)
                .WithOne()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(t => t.Person)
                .WithOne()
                .IsRequired()
                //.HasForeignKey(nameof(CustomerModel.PersonModelId))
                .OnDelete(DeleteBehavior.ClientCascade);

            entity.HasOne(t => t.LoginCredential)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(CustomerModel.LoginCredentialModelId))
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.Property(e => e.EntryDate)
                .HasColumnName("Created on")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());
        }
    }
}
