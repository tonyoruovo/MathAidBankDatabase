using DataBaseTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseTest.Data
{
    internal class TransactionConfig : IEntityTypeConfiguration<TransactionModel>
    {
        public void Configure(EntityTypeBuilder<TransactionModel> entity)
        {
            entity.Property<DateTime>("LastUpdatedOn")
                .HasColumnName("Last updated")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());

            entity.HasKey(n => n.TransactionModelId);
            entity.ToTable("Transactions");

            entity.HasOne(t => t.Creditor)
                .WithOne()
                .IsRequired()
                //.HasForeignKey(nameof(TransactionModel.CreditorEntityModelId))
                .OnDelete(DeleteBehavior.ClientCascade);

            entity.HasOne(t => t.Debitor)
                .WithOne()
                .IsRequired(false)
                //.HasForeignKey(nameof(TransactionModel.DebitorEntityModelId))
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.Property(e => e.Amount)
                .HasColumnName("Amount")
                .HasColumnType("money")
                .IsRequired();

            entity.Property(e => e.Date)
                .HasColumnName("Transaction date")
                .HasColumnType("DateTime2")
                .HasConversion(Utilities.UtcConverter, Utilities.Comparer<DateTime>());

            entity.Property(e => e.Currency)
                .HasConversion(Utilities.JsonCurrencyConverter(), Utilities.Comparer<ICurrency>())
                .HasColumnName("Currency")
                .IsRequired(false);
        }
    }
}
