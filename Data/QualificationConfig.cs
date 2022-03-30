using DataBaseTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseTest.Data
{
    internal class QualificationConfig : IEntityTypeConfiguration<QualificationModel>
    {
        public void Configure(EntityTypeBuilder<QualificationModel> entity)
        {

            entity.Property<DateTime>("LastUpdatedOn")
                .HasColumnName("Last updated")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());

            entity.HasKey(n => n.QualificationModelId);
            entity.ToTable("Qualifications");

            entity.HasOne(e => e.Academy)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(QualificationModel.AcademyAddressModelId))
                .OnDelete(DeleteBehavior.ClientCascade);

            entity.Property(c => c.Grades)
            .HasConversion(Utilities.JsonStringConverter2, Utilities.DictionaryComparer)
            .HasColumnName("Grade(s)")
            .IsRequired(false);

        }
    }
}
