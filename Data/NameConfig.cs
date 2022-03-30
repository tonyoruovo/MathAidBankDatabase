using DataBaseTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseTest.Data
{
    internal class NameConfig : IEntityTypeConfiguration<NameModel>
    {
        public void Configure(EntityTypeBuilder<NameModel> entity)
        {
            entity.Property<DateTime>("LastUpdatedOn")
                .HasColumnName("Last updated")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());

            entity.HasKey(n => n.NameModelId);
            entity.ToTable("Names");

            entity.Property(n => n.Name)
                .IsRequired(true)
                .HasColumnName("Name")
                .HasColumnType("NVARCHAR(32)")
                .IsUnicode(true);
        }
    }
}
