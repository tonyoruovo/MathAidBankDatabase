using DataBaseTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseTest.Data
{
    internal class LoginCredentialsConfig : IEntityTypeConfiguration<LoginCredentialModel>
    {
        public void Configure(EntityTypeBuilder<LoginCredentialModel> entity)
        {

            entity.Property<DateTime>("LastUpdatedOn")
                .HasColumnName("Last updated")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());

            entity.HasKey(n => n.LoginCredentialModelId);
            entity.ToTable("Login credentials");

            entity.Property(n => n.PersonalOnlineKey)
                .HasColumnName("Personal online key")
                .IsUnicode(false);

            entity.Property(n => n.Username)
                .HasColumnName("Username");

            entity.Property(n => n.Password)
                .HasColumnName("Password");

            entity.Property(c => c.Tokens)
            .HasConversion(Utilities.JsonStringConverter, Utilities.ListComparer<string>())
            .HasColumnName("Token(s)")
            .IsRequired(false);

        }
    }
}
