using DataBaseTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseTest.Data
{
    internal class AddressConfig : IEntityTypeConfiguration<AddressModel>
    {
        public void Configure(EntityTypeBuilder<AddressModel> entity)
        {

            entity.Property<DateTime>("LastUpdatedOn")
                .HasColumnName("Last updated")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());

            entity.HasKey(n => n.AddressModelId);
            entity.ToTable("Addresses");

            entity.HasIndex(n => new { n.StreetAddress });

            entity.HasOne(n => n.CountryOfResidence)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(AddressModel.CountryOfResidenceNationalityModelId))
                .OnDelete(DeleteBehavior.ClientCascade);

            entity.Property(n => n.PMB)
                .IsRequired(false)
                .IsUnicode(false)
                .HasColumnName("P.M.B");

            entity.Property(n => n.StreetAddress)
                .IsRequired()
                .HasColumnName("Street address");

            entity.Property(n => n.ZIPCode)
                .IsRequired(false)
                .HasColumnName("Z.I.P code")
                .IsUnicode(false);

        }
    }
}
