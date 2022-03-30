using DataBaseTest.Models;
using DataBaseTest.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace DataBaseTest.Data
{
    internal class AccountConfig : IEntityTypeConfiguration<AccountModel>
    {
        public void Configure(EntityTypeBuilder<AccountModel> entity)
        {
            entity.Property<DateTime>("LastUpdatedOn")
                .HasColumnName("Last updated")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());

            entity.HasKey(n => n.AccountModelId);
            entity.ToTable("Accounts");

            entity.HasMany(t => t.Transactions)
                .WithOne()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasMany(t => t.Cards)
                .WithOne()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(t => t.Guarantor)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(AccountModel.GuarantorPersonModelId))
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.Property(e => e.EntryDate)
                .HasColumnName("Created on")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());

            entity.Property(e => e.Balance)
                .HasColumnName("Balance")
                .HasColumnType("money")
                .IsRequired();

            entity.Property(e => e.PercentageIncrease)
                .HasColumnType("money")
                .IsRequired();

            entity.Property(e => e.PercentageDecrease)
                .HasColumnType("money")
                .IsRequired();

            entity.Property(e => e.Debt)
                .HasColumnType("money")
                .IsRequired();

            entity.Property(e => e.CreditLimit)
                .HasColumnType("money")
                .IsRequired();

            entity.Property(e => e.DebitLimit)
                .HasColumnType("money")
                .IsRequired();

            entity.Property(e => e.SmallestBalance)
                .HasColumnType("money")
                .IsRequired();

            entity.Property(e => e.SmallestTransferIn)
                .HasColumnType("money")
                .IsRequired();

            entity.Property(e => e.SmallestTransferOut)
                .HasColumnType("money")
                .IsRequired();

            entity.Property(e => e.LargestBalance)
                .HasColumnType("money")
                .IsRequired();

            entity.Property(e => e.LargestTransferIn)
                .HasColumnType("money")
                .IsRequired();

            entity.Property(e => e.LargestTransferOut)
                .HasColumnType("money")
                .IsRequired();

            entity.Property(e => e.Currency)
                .HasConversion(Utilities.JsonCurrencyConverter(), Utilities.Comparer<ICurrency>())
                .HasColumnName("Currency")
                .IsRequired(false);

            entity.Property(c => c.SMSAlertList)
                .HasConversion(Utilities.JsonStringConverter, Utilities.ListComparer<string>())
                .HasColumnName("SMS Number(s)")
                .IsRequired(false);

            entity.Property(c => c.EmailAlertList)
                .HasConversion(Utilities.JsonStringConverter, Utilities.ListComparer<string>())
                .HasColumnName("Email(s)")
                .IsRequired(false);

            entity.Property(c => c.Statuses)
                .HasConversion(Utilities.JsonStringConverter3<Queue<AccountStatusInfo>, string>(), Utilities.QueueComparer<AccountStatusInfo>())
                .HasColumnName("Status(es)")
                .IsRequired(false);
        }
    }
}
