using DataBaseTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseTest.Data
{
    internal class ContactConfig : IEntityTypeConfiguration<ContactModel>
    {
        public void Configure(EntityTypeBuilder<ContactModel> entity)
        {

            entity.Property<DateTime>("LastUpdatedOn")
                .HasColumnName("Last updated")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());

            entity.HasKey(n => n.ContactModelId);
            entity.ToTable("Contacts");

            entity.HasOne(n => n.Address)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(ContactModel.AddressModelId))
                .OnDelete(DeleteBehavior.ClientCascade);

            entity.Property(c => c.Mobiles)
                .HasConversion(Utilities.JsonStringConverter, Utilities.ListComparer<string>())
                .HasColumnName("Mobile(s)")
                .IsRequired();

            entity.Property(c => c.Emails)
                .HasConversion(Utilities.JsonStringConverter, Utilities.ListComparer<string>())
                .HasColumnName("Email(s)")
                .IsRequired(false);

            entity.Property(c => c.SocialMedia)
                .HasConversion(Utilities.JsonStringConverter, Utilities.ListComparer<string>())
                .HasColumnName("Social media")
                .IsRequired(false);

            entity.Property(c => c.Websites)
                .HasConversion(Utilities.JsonStringConverter, Utilities.ListComparer<string>())
                .HasColumnName("Website(s)")
                .IsRequired(false);

        }
    }
}
