using DataBaseTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseTest.Data
{
    internal class EntityConfig : IEntityTypeConfiguration<EntityModel>
    {
        public void Configure(EntityTypeBuilder<EntityModel> entity)
        {

            entity.Property<DateTime>("LastUpdatedOn")
                .HasColumnName("Last updated")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());

            entity.HasKey(n => n.EntityModelId);
            entity.ToTable("Entities");
            //entity.HasIndex(e => e.Name);

            entity.HasOne(n => n.Name)
                .WithOne()
                .IsRequired()
                //.HasForeignKey(nameof(EntityModel.NameModelIdId))
                .OnDelete(DeleteBehavior.ClientCascade);

            entity.HasOne(n => n.Contact)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(EntityModel.ContactModelIdId))
                .OnDelete(DeleteBehavior.ClientSetNull);

            //entity.OwnsOne(n => n.Contact).WithOwner();

        }
    }
}
