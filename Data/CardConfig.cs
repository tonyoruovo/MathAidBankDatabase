using DataBaseTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseTest.Data
{
    internal class CardConfig : IEntityTypeConfiguration<CardModel>
    {
        public void Configure(EntityTypeBuilder<CardModel> entity)
        {
            entity.Property<DateTime>("LastUpdatedOn")
                .HasColumnName("Last updated")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());

            entity.HasKey(n => n.CardModelId);
            entity.ToTable("Cards");

            entity.HasOne(t => t.Brand)
                .WithOne()
                .IsRequired()
                //.HasForeignKey(nameof(CardModel.BrandEntityModelId))
                .OnDelete(DeleteBehavior.ClientCascade);

            entity.HasOne(t => t.CountryOfUse)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(CardModel.CountryOfUseNationalityModelId))
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(t => t.Secret)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(CardModel.SecretLoginCredentialModelId))
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.Property(e => e.IssuedDate)
                .HasColumnName("Issued on")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());

            entity.Property(e => e.ExpiryDate)
                .HasColumnName("Expires on")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());

            entity.Property(e => e.IssuedCost)
                .HasColumnName("Issued cost")
                .HasColumnType("money")
                .IsRequired();

            entity.Property(e => e.MonthlyRate)
                .HasColumnName("Monthly rate")
                .HasColumnType("money")
                .IsRequired();

            entity.Property(e => e.WithdrawalLimit)
                .HasColumnName("Withdrawal limit")
                .HasColumnType("money")
                .IsRequired();

            entity.Property(e => e.Currency)
                .HasConversion(Utilities.JsonCurrencyConverter(), Utilities.Comparer<ICurrency>())
                .HasColumnName("Currency")
                .IsRequired(false);
        }
    }
}
