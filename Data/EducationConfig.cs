using DataBaseTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseTest.Data
{
    internal class EducationConfig : IEntityTypeConfiguration<EducationModel>
    {
        public void Configure(EntityTypeBuilder<EducationModel> entity)
        {

            entity.Property<DateTime>("LastUpdatedOn")
                .HasColumnName("Last updated")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());

            entity.HasKey(n => n.EducationModelId);
            entity.ToTable("Education credentials");

            //entity.OwnsOne(e => e.Primary).WithOwner();

            entity.HasOne(e => e.Primary)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(EducationModel.PrimaryQualificationModelId))
                .OnDelete(DeleteBehavior.ClientSetNull);

            //entity.OwnsOne(e => e.Secondary).WithOwner();

            entity.HasOne(e => e.Secondary)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(EducationModel.SecondaryQualificationModelId))
                .OnDelete(DeleteBehavior.ClientSetNull);

            //entity.OwnsOne(e => e.PrimaryTertiary).WithOwner();

            entity.HasOne(e => e.PrimaryTertiary)
                .WithOne()
                .IsRequired()
                //.HasForeignKey(nameof(EducationModel.PrimaryTertiaryQualificationModelId))
                .OnDelete(DeleteBehavior.ClientCascade);

            //entity.OwnsMany(e => e.Others).WithOwner();
                //.HasForeignKey(nameof(EducationModel.PrimaryQualificationModelId));

            entity.HasMany(e => e.Others);

        }
    }
}
