using DataBaseTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseTest.Data
{
    internal class PersonConfig : IEntityTypeConfiguration<PersonModel>
    {
        public void Configure(EntityTypeBuilder<PersonModel> entity)
        {

            entity.Property<DateTime>("LastUpdatedOn")
                .HasColumnName("Last updated")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());

            entity.HasKey(n => n.PersonModelId);
            entity.ToTable("People");
            //entity.HasIndex(p => new { p.FullName.Name.Name });

            entity.HasOne(n => n.Entity)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(PersonModel.EntityModelId))
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(n => n.OfficialEntity)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(PersonModel.OfficialEntityEntityModelId))
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(n => n.FullName)
                .WithOne()
                .IsRequired()
                //.HasForeignKey(nameof(PersonModel.FullNameModelId))
                .OnDelete(DeleteBehavior.ClientCascade);

            //entity.OwnsOne(n => n.FullName).WithOwner();

            entity.HasOne(n => n.CountryOfOrigin)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(PersonModel.CountryOfOriginNationalityModelId))
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.Property("_birthDate")
                .HasColumnName("Birth date")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());

            entity.Property(c => c.NextOfKin)
                .HasConversion(Utilities.JsonStringConverter3<PersonModel, string>(), Utilities.Comparer<PersonModel>())
                .HasColumnName("Next of kin")
                .IsRequired(false);

            entity.Property(c => c.Identification)
                .HasConversion(Utilities.JsonStringConverter2, Utilities.DictionaryComparer)
                .HasColumnName("Identification(s)")
                .IsRequired(false);

        }
    }
}
