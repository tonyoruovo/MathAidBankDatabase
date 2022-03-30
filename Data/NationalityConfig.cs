using DataBaseTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseTest.Data
{
    internal class NationalityConfig : IEntityTypeConfiguration<NationalityModel>
    {
        public void Configure(EntityTypeBuilder<NationalityModel> entity)
        {

            entity.Property<DateTime>("LastUpdatedOn")
                .HasColumnName("Last updated")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());

            entity.HasKey(n => n.NationalityModelId);
            entity.ToTable("Nationalities");

            //entity.HasIndex(n => new { n.CountryName, n.State, n.CityTown, n.Language, n.LGA });

            entity.HasOne(n => n.CountryName)
                .WithOne()
                .IsRequired()
                //.HasForeignKey(nameof(NationalityModel.CountryNameNameModelId))
                .OnDelete(DeleteBehavior.ClientCascade);

            entity.HasOne(n => n.State)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(NationalityModel.StateNameModelId))
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(n => n.CityTown)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(NationalityModel.CityTownNameModelId))
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.Property(n => n.Language)
                .IsRequired(false)
                .HasColumnName("Language")
                .IsUnicode(false);

            entity.Property(n => n.LGA)
                .IsRequired(false)
                .HasColumnName("Local Government Area");

        }
    }
}
