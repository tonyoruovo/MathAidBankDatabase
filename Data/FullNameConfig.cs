using DataBaseTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace DataBaseTest.Data
{
    internal class FullNameConfig : IEntityTypeConfiguration<FullNameModel>
    {
        public void Configure(EntityTypeBuilder<FullNameModel> entity)
        {
            entity.Property<DateTime>("LastUpdatedOn")
                .HasColumnName("Last updated")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());

            entity.HasKey(n => n.FullNameModelId);

            entity.HasOne(f => f.Name)
                .WithOne()
                .IsRequired()
                //.HasForeignKey(nameof(FullNameModel.NameModelId))
                .OnDelete(DeleteBehavior.ClientCascade);

            entity.HasOne(f => f.Surname)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(FullNameModel.SurnameNameModelId))
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(f => f.MaidenName)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(FullNameModel.MaidenNameNameModelId))
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(f => f.Nickname)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(FullNameModel.NicknameNameModelId))
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.ToTable("FullNames");

            //entity.HasIndex(f => $"{f.Name.Name} {f.Surname.Name}" );

            //entity.Ignore(f => f.OtherNames);

            entity.Property(f => f.OtherNames)
                //.HasField("_otherNames");//Does not need a backing field cause it been coverted
                .HasConversion(Utilities.JsonStringConverter3<List<NameModel>, string>(), Utilities.ListComparer<NameModel>())
                .HasColumnName("Other name(s)")
                .IsRequired(false);

            entity.Property(n => n.Title)
                .IsRequired(false)
                .HasColumnName("Titles");
        }
    }
}
